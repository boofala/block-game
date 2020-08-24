using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RollMovement : MonoBehaviour
{
    private enum Movement
    {
        Up,
        Down,
        Left,
        Right,
        Jump,
        Reset,
        None
    }

    public HealthBar healthBar;

    public int rowStart, columnStart;

    public GridManager gridManager;
    public OccupancyManager occupancyManager;
    public UIManager uiManager;

    public UIManager.Player playerNum;
    public Color blockColor;

    public Color tileColor;

    public GameObject player;

    // Buffer
    private Queue<Movement> inputBuffer;
    private List<Movement> oldInputs;
    private int framesInBuffer = -1;
    private readonly int bufferMaxFrames = GameSettings.BUFFER_MAX_FRAMES;

    // GameEnd
    private bool gameEnd = false;
    private bool gameFinish = false;

    //Text
    public Text restartText;
    public Text continueText;
    public Text score1;
    public Text score2;

    // Points
    private GameObject center;
    private GameObject jumpCenter;

    private GameObject rightUp;
    private GameObject leftDown;

    private GameObject rightJumpPoint;
    private GameObject leftJumpPoint;
    private GameObject upJumpPoint;
    private GameObject downJumpPoint;

    // Health
    private float currentHealth;
    private float damage;
    private int sedentaryThreshold = GameSettings.SEDENTARY_THRESHOLD;
    private int sedentaryCount = 0;
    private int lives = GameSettings.gameLength;

    // Jump Parameters
    private readonly int jumpDelay = GameSettings.JUMP_DELAY;
    private readonly int jumpRotationSpeed = GameSettings.JUMP_ROTATION_SPEED;
    private readonly int jumpWobbleDegrees = GameSettings.JUMP_WOBBLE_DEGREES;
    private readonly int stepDistance = GameSettings.STEP_DISTANCE;
    private readonly int jumpDistance = GameSettings.JUMP_DISTANCE;

    // Rotation Parameters
    private int degreesInStep = GameSettings.DEGREES_IN_STEP;

    // Grid Variables
    private int currRow, currColumn;
    private Color boardColor;

    // Input Variables
    private Dictionary<Movement, bool> movementState = new Dictionary<Movement, bool>()
    {
        {Movement.Up, false},
        {Movement.Down, false},
        {Movement.Left, false},
        {Movement.Right, false},
    };
    private bool moveInput = true;
    private bool jumpInput = true;
    private bool addScore = true;

    private void Start()
    {
        this.healthBar.SetMaxHealth(GameSettings.MAX_HEALTH);
        this.damage = GameSettings.DAMAGE;
        this.currentHealth = GameSettings.MAX_HEALTH;
        Application.targetFrameRate = GameSettings.FRAME_RATE;

        // Instantiate GameObjects
        this.center = new GameObject();
        this.jumpCenter = new GameObject();
        this.rightUp = new GameObject();
        this.leftDown = new GameObject();
        this.rightJumpPoint = new GameObject();
        this.leftJumpPoint = new GameObject();
        this.upJumpPoint = new GameObject();
        this.downJumpPoint = new GameObject();

        //instantiate text
        continueText.text = "";
        restartText.text = "";
        score1.text = "";
        score2.text = "";

        // Input Buffer
        inputBuffer = new Queue<Movement>();

        // Set Position
        this.currRow = this.rowStart;
        this.currColumn = this.columnStart;
        Vector3 startOffset = new Vector3(0f, 0.5f, 0f);
        Vector3 startPoint = this.gridManager.GetPosition(rowStart, columnStart) + startOffset;
        this.transform.position = 
            this.center.transform.position = 
            this.jumpCenter.transform.position =
            startPoint;
        this.rightUp.transform.position = startPoint + new Vector3(stepDistance / 2f, -stepDistance / 2f, stepDistance / 2f);
        this.leftDown.transform.position = startPoint + new Vector3(-stepDistance / 2f, -stepDistance / 2f, -stepDistance / 2f);
        this.rightJumpPoint.transform.position = startPoint + new Vector3(jumpDistance / 2f, 0, 0);
        this.leftJumpPoint.transform.position = startPoint + new Vector3(-jumpDistance / 2f, 0, 0);
        this.upJumpPoint.transform.position = startPoint + new Vector3(0, 0, jumpDistance / 2f);
        this.downJumpPoint.transform.position = startPoint + new Vector3(0, 0, -jumpDistance / 2f);
        this.rightUp.transform.parent =
            this.rightUp.transform.parent =
            this.leftDown.transform.parent =
            this.rightJumpPoint.transform.parent =
            this.leftJumpPoint.transform.parent =
            this.upJumpPoint.transform.parent =
            this.downJumpPoint.transform.parent =
            center.transform;

        // Set Colors
        SetBlockColor(this.blockColor);
        boardColor = GetTileColor();
        /*if (player.name == "Player1")
        {
            score1.color = this.blockColor;
        } else
        {
            score2.color = this.blockColor;
        }*/
    }

    void Update()
    {
        
        // Game States
        if (gameEnd)
        {
            jumpInput = false;
            moveInput = false;
            if (addScore)
            {
                uiManager.UpdateScore(this.playerNum);
                addScore = false;
            }
            score1.text = GameSettings.scores[UIManager.Player.Player1].ToString();
            score2.text = GameSettings.scores[UIManager.Player.Player2].ToString();
            if (GameSettings.scores[UIManager.Player.Player1] == 0 || GameSettings.scores[UIManager.Player.Player2] == 0)
            {
                gameEnd = false;
                continueText.text = player.name.Substring(0, 6) + " " + player.name.Substring(player.name.Length - 1) + " Loses!";
                restartText.text = "Press 'N' or 'Select' to Restart";
                gameFinish = true;
            }
            else
            {
                continueText.text = player.name.Substring(0, 6) + " " + player.name.Substring(player.name.Length - 1) + ": Press Jump to Continue";
                restartText.text = "Press 'N' or 'Select' to Restart";
            }
        }

        // Damage
        sedentaryCount++;
        if (GetTileColor() == this.tileColor || sedentaryCount > sedentaryThreshold)
        {
            TakeDamage(damage);
        }

        List<Movement> inputs = GetBufferedInputs();
        BufferInput(Movement.None);
        if (inputs == null) return;
        oldInputs = inputs;
        sedentaryCount = 0;
        // Up Movement
        if (jumpInput && moveInput && inputs.Contains(Movement.Jump) && inputs.Contains(Movement.Up))
        {
            bool isOccupied = occupancyManager.getOccupancy(currRow - jumpDistance, currColumn);
            if (!isOccupied)
            {
                SetTileColor();
                currRow -= jumpDistance;
                StartCoroutine(jump(upJumpPoint, rightUp, Vector3.right, Vector3.left));
                jumpInput = false;
                moveInput = false;
            }
        }
        if (moveInput && inputs.Contains(Movement.Up))
        {
            bool isOccupied = occupancyManager.getOccupancy(currRow - stepDistance, currColumn);
            if (!isOccupied)
            {
                SetTileColor();
                currRow -= stepDistance;
                StartCoroutine(move(rightUp, Vector3.right));
                moveInput = false;
            }
        }
        // Down Movement
        if (jumpInput && moveInput && inputs.Contains(Movement.Jump) && inputs.Contains(Movement.Down))
        {
            bool isOccupied = occupancyManager.getOccupancy(currRow + jumpDistance, currColumn);
            if (!isOccupied)
            {
                SetTileColor();
                currRow += jumpDistance;
                StartCoroutine(jump(downJumpPoint, leftDown, Vector3.left, Vector3.right));
                jumpInput = false;
                moveInput = false;
            }
        }
        if (moveInput && inputs.Contains(Movement.Down))
        {
            bool isOccupied = occupancyManager.getOccupancy(currRow + stepDistance, currColumn);
            if (!isOccupied)
            {
                SetTileColor();
                currRow += stepDistance;
                StartCoroutine(move(leftDown, Vector3.left));
                moveInput = false;
            }
        }
        //Left Movement
        if (jumpInput && moveInput && inputs.Contains(Movement.Jump) && inputs.Contains(Movement.Left))
        {
            bool isOccupied = occupancyManager.getOccupancy(currRow, currColumn - jumpDistance);
            if (!isOccupied)
            {
                SetTileColor();
                currColumn -= jumpDistance;
                StartCoroutine(jump(leftJumpPoint, leftDown, Vector3.forward, Vector3.back));
                jumpInput = false;
                moveInput = false;
            }
        }
        if (moveInput && inputs.Contains(Movement.Left))
        {
            bool isOccupied = occupancyManager.getOccupancy(currRow, currColumn - stepDistance);
            if (!isOccupied)
            {
                SetTileColor();
                currColumn -= stepDistance;
                StartCoroutine(move(leftDown, Vector3.forward));
                moveInput = false;
            }
        }
        // Right Movement
        if (jumpInput && moveInput && inputs.Contains(Movement.Jump) && inputs.Contains(Movement.Right))
        {
            bool isOccupied = occupancyManager.getOccupancy(currRow, currColumn + jumpDistance);
            if (!isOccupied)
            {
                SetTileColor();
                currColumn += jumpDistance;
                StartCoroutine(jump(rightJumpPoint, rightUp, Vector3.back, Vector3.forward));
                jumpInput = false;
                moveInput = false;
            }
        }
        if (moveInput && inputs.Contains(Movement.Right))
        {
            bool isOccupied = occupancyManager.getOccupancy(currRow, currColumn + stepDistance);
            if (!isOccupied)
            {
                SetTileColor();
                currColumn += stepDistance;
                StartCoroutine(move(rightUp, Vector3.back));
                moveInput = false;
            }
        }
    }

    private void TakeDamage(float damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        if (currentHealth < 0)
        {
            gameEnd = true;
        }
    }

    public void OnUp() {
        movementState[Movement.Up] ^= true;
        if (movementState[Movement.Up])
        {
            BufferInput(Movement.Up);
        }
        
    }

    public void OnDown() {
        movementState[Movement.Down] ^= true;
        if (movementState[Movement.Down])
        {
            BufferInput(Movement.Down);
        }
    }

    public void OnLeft() {
        movementState[Movement.Left] ^= true;
        if (movementState[Movement.Left])
        {
            BufferInput(Movement.Left);
        }
    }

    public void OnRight() {
        movementState[Movement.Right] ^= true;
        if (movementState[Movement.Right])
        {
            BufferInput(Movement.Right);
        }
    }

    public void OnJump() {
        if (gameEnd)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        } else
        {
            BufferInput(Movement.Jump);
        }
    }

    public void OnRestart()
    {
        if (gameEnd || gameFinish)
        {
            gameEnd = false;
            score1.text = "";
            score2.text = "";
            GameSettings.scores[UIManager.Player.Player1] = lives;
            GameSettings.scores[UIManager.Player.Player2] = lives;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void BufferInput(Movement movement)
    {
        if (movement == Movement.None && oldInputs != null)
        {
            // Check old inputs to see if they're still pressed
            foreach (Movement oldMovement in oldInputs)
            {
                if (oldMovement != Movement.None && oldMovement != Movement.Jump && movementState[oldMovement])
                {
                    if (framesInBuffer == -1)
                    {
                        framesInBuffer = 0;
                    }
                    inputBuffer.Enqueue(oldMovement);
                }
            }
        } else
        {
            if (framesInBuffer == -1)
            {
                framesInBuffer = 0;
            }
            inputBuffer.Enqueue(movement);
        }
    }

    private List<Movement> GetBufferedInputs()
    {

        if (framesInBuffer >= 0)
        {
            framesInBuffer++;
        }
        if (framesInBuffer > bufferMaxFrames)
        {
            framesInBuffer = -1;
            Movement firstInput = inputBuffer.Dequeue();
            Movement secondInput = Movement.None;
            if (inputBuffer.Count >= 1)
            {
                secondInput = inputBuffer.Dequeue();
            }
            inputBuffer.Clear();
            return new List<Movement> { firstInput, secondInput };
        }
        else return null;
    }

    // Set Colors
    private void SetBlockColor(Color color)
    {
        var renderer = this.GetComponent<Renderer>();
        renderer.material.SetColor("_Color", color);
        renderer.material.SetColor("_EmissionColor", color);
    }

    private void SetTileColor()
    {
        gridManager.SetColor(currRow, currColumn, this.tileColor);
    }

    // Get Colors
    private Color GetTileColor()
    {
        return gridManager.GetColor(currRow, currColumn);
    }

    // Move
    IEnumerator move(GameObject point, Vector3 direction)
    {
        int framesInMove = 90 / degreesInStep;
        for (int i = 0; i < framesInMove; i ++)
        {
            player.transform.RotateAround(point.transform.position, direction, degreesInStep);
            yield return null;
        }
        center.transform.position = player.transform.position;
        if (GetTileColor() != boardColor && GetTileColor() != tileColor)
        {
            gameEnd = true;
        }
        moveInput = true;
        sedentaryCount = 0;
    }

    IEnumerator jump(GameObject jumpPoint, GameObject point, Vector3 direction, Vector3 wobbleDirection)
    {
        for (int i =0; i < (180 / degreesInStep); i++)
        {
            player.transform.RotateAround(jumpPoint.transform.position, direction, degreesInStep);
            jumpCenter.transform.position = player.transform.position;
            player.transform.RotateAround(jumpCenter.transform.position, direction, degreesInStep * jumpRotationSpeed);
            yield return null;
        }
        for (int i = 0; i < (jumpWobbleDegrees / degreesInStep); i++)
        {
            player.transform.RotateAround(point.transform.position, direction, degreesInStep);
            yield return null;
        }
        for (int i = 0; i < (jumpWobbleDegrees / degreesInStep); i++)
        {
            player.transform.RotateAround(point.transform.position, wobbleDirection, degreesInStep);
            yield return null;
        }
        center.transform.position = player.transform.position;
        if (GetTileColor() != boardColor && GetTileColor() != tileColor)
        {
            gameEnd = true;
        }
        yield return null;
        jumpInput = true;
        moveInput = true;
        sedentaryCount = 0;
    }

}
