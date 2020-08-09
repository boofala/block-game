using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RollMovement : MonoBehaviour
{
    public int rowStart, columnStart;

    public GridManager gridManager;
    public OccupancyManager occupancyManager;

    public Color blockColor;

    public Color tileColor;

    public GameObject player;

    // Points
    private GameObject center;
    private GameObject jumpCenter;

    private GameObject rightUp;
    private GameObject leftDown;

    private GameObject rightJumpPoint;
    private GameObject leftJumpPoint;
    private GameObject upJumpPoint;
    private GameObject downJumpPoint;

    public bool isWasd;

    // Jump Parameters
    private int jumpDelay = GameSettings.JUMP_DELAY;
    private int jumpRotationSpeed = GameSettings.JUMP_ROTATION_SPEED;
    private int jumpWobbleDegrees = GameSettings.JUMP_WOBBLE_DEGREES;
    private int stepDistance = GameSettings.STEP_DISTANCE;
    private int jumpDistance = GameSettings.JUMP_DISTANCE;

    // Rotation Parameters
    private int degreesInStep = GameSettings.DEGREES_IN_STEP;
    
    // Grid Variables
    private int currRow, currColumn;
    private Color boardColor;
    private bool gameEnd = false;

    // Input Variables
    private bool moveInput = true;
    private bool jumpInput = true;

    private Dictionary<string, KeyCode> wasdKeys = new Dictionary<string, KeyCode>
    {
        { "up", KeyCode.W },
        { "down", KeyCode.S },
        { "left", KeyCode.A },
        { "right", KeyCode.D },
        { "jump", KeyCode.Space }
    };

    private Dictionary<string, KeyCode> arrowKeys = new Dictionary<string, KeyCode>
    {
        { "up", KeyCode.UpArrow },
        { "down", KeyCode.DownArrow },
        { "left", KeyCode.LeftArrow },
        { "right", KeyCode.RightArrow },
        { "jump", KeyCode.KeypadEnter }
    };

    private Dictionary<string, KeyCode> keys;

    private void Start()
    {
        Application.targetFrameRate = GameSettings.FRAME_RATE;

        // instantiate GameObjects
        this.center = new GameObject();
        this.jumpCenter = new GameObject();
        this.rightUp = new GameObject();
        this.leftDown = new GameObject();
        this.rightJumpPoint = new GameObject();
        this.leftJumpPoint = new GameObject();
        this.upJumpPoint = new GameObject();
        this.downJumpPoint = new GameObject();


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
        SetTileColor();

        // Set Keys
        if (this.isWasd)
        {
            this.keys = wasdKeys;
        }
        else
        {
            this.keys = arrowKeys;
        }
    }

    void Update()
    {
        // Up Movement
        if (gameEnd)
        {
            //show text
            Application.Quit();
            //SceneManager.LoadScene(SceneManager.GetActiveScene().ToString());
        }
        if (jumpInput && moveInput && Input.GetKey(this.keys["jump"]) && Input.GetKey(this.keys["up"]))
        {
            bool isOccupied = occupancyManager.getOccupancy(currRow - jumpDistance, currColumn);
            if (!isOccupied)
            {
                currRow -= jumpDistance;
                StartCoroutine(jump(upJumpPoint, rightUp, Vector3.right, Vector3.left));
                jumpInput = false;
                moveInput = false;
            }
        }
        if (moveInput && Input.GetKey(this.keys["up"]))
        {
            bool isOccupied = occupancyManager.getOccupancy(currRow - stepDistance, currColumn);
            if (!isOccupied)
            {
                currRow -= stepDistance;
                StartCoroutine(move(rightUp, Vector3.right));
                moveInput = false;
            }
        }
        // Down Movement
        if (jumpInput && moveInput && Input.GetKey(this.keys["jump"]) && Input.GetKey(this.keys["down"]))
        {
            bool isOccupied = occupancyManager.getOccupancy(currRow + jumpDistance, currColumn);
            if (!isOccupied)
            {
                currRow += jumpDistance;
                StartCoroutine(jump(downJumpPoint, leftDown, Vector3.left, Vector3.right));
                jumpInput = false;
                moveInput = false;
            }
        }
        if (moveInput && Input.GetKey(this.keys["down"]))
        {
            bool isOccupied = occupancyManager.getOccupancy(currRow + stepDistance, currColumn);
            if (!isOccupied)
            {
                currRow += stepDistance;
                StartCoroutine(move(leftDown, Vector3.left));
                moveInput = false;
            }
        }
        //Left Movement
        if (jumpInput && moveInput && Input.GetKey(this.keys["jump"]) && Input.GetKey(this.keys["left"]))
        {
            bool isOccupied = occupancyManager.getOccupancy(currRow, currColumn - jumpDistance);
            if (!isOccupied)
            {
                currColumn -= jumpDistance;
                StartCoroutine(jump(leftJumpPoint, leftDown, Vector3.forward, Vector3.back));
                jumpInput = false;
                moveInput = false;
            }
        }
        if (moveInput && Input.GetKey(this.keys["left"]))
        {
            bool isOccupied = occupancyManager.getOccupancy(currRow, currColumn - stepDistance);
            if (!isOccupied)
            {
                currColumn -= stepDistance;
                StartCoroutine(move(leftDown, Vector3.forward));
                moveInput = false;
            }
        }
        // Right Movement
        if (jumpInput && moveInput && Input.GetKey(this.keys["jump"]) && Input.GetKey(this.keys["right"]))
        {
            bool isOccupied = occupancyManager.getOccupancy(currRow, currColumn + jumpDistance);
            if (!isOccupied)
            {
                currColumn += jumpDistance;
                StartCoroutine(jump(rightJumpPoint, rightUp, Vector3.back, Vector3.forward));
                jumpInput = false;
                moveInput = false;
            }
        }
        if (moveInput && Input.GetKey(this.keys["right"]))
        {
            bool isOccupied = occupancyManager.getOccupancy(currRow, currColumn + stepDistance);
            if (!isOccupied)
            {
                currColumn += stepDistance;
                StartCoroutine(move(rightUp, Vector3.back));
                moveInput = false;
            }
        }
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
        for (int i = 0; i < (90 / degreesInStep); i ++)
        {
            player.transform.RotateAround(point.transform.position, direction, degreesInStep);
            yield return null;
        }
        center.transform.position = player.transform.position;
        if (GetTileColor() != boardColor)
            {
            gameEnd = true;
            }
        moveInput = true;
        SetTileColor();
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
        yield return null;
        jumpInput = true;
        moveInput = true;
        SetTileColor();
    }

}
