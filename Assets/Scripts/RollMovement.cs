using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollMovement : MonoBehaviour
{
    public int rowStart, columnStart;

    public GridManager gridManager;

    public GameObject player;

    //Points
    public GameObject center;
    public GameObject jumpCenter;
   
    public GameObject rightUp;
    public GameObject leftDown;

    public GameObject rightJumpPoint;
    public GameObject leftJumpPoint;
    public GameObject upJumpPoint;
    public GameObject downJumpPoint;

    public bool isWasd;

    //Movement Parameters
    public float speed = 0.01f;

    //Jump Parameters
    public int jumpDelay = 0;
    public int jumpRotationSpeed = 2;
    public int jumpWobbleDegrees = 5;

    //Rotation Parameters
    public int degreesInStep = 9;
    
    //Grid Variables
    private int currRow, currColumn;
    private int stepDistance = 1;
    private int jumpDistance = 4;

    //Input Variables
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
        // set position
        this.currRow = this.rowStart;
        this.currColumn = this.columnStart;
        this.transform.position = 
            this.center.transform.position = 
            this.jumpCenter.transform.position = 
            (this.gridManager.GetPosition(rowStart, columnStart) + new Vector3(0f, 0.5f, 0f));
        this.rightUp.transform.position = (this.gridManager.GetPosition(rowStart, columnStart) + new Vector3(0f, 0.5f, 0f) + new Vector3(stepDistance / 2f, -stepDistance / 2f, stepDistance / 2f));
        this.leftDown.transform.position = (this.gridManager.GetPosition(rowStart, columnStart) + new Vector3(0f, 0.5f, 0f) + new Vector3(-stepDistance / 2f, -stepDistance / 2f, -stepDistance / 2f));
        this.rightJumpPoint.transform.position = (this.gridManager.GetPosition(rowStart, columnStart) + new Vector3(0f, 0.5f, 0f) + new Vector3(jumpDistance / 2f, 0, 0));
        this.leftJumpPoint.transform.position = (this.gridManager.GetPosition(rowStart, columnStart) + new Vector3(0f, 0.5f, 0f) + new Vector3(-jumpDistance / 2f, 0, 0));
        this.upJumpPoint.transform.position = (this.gridManager.GetPosition(rowStart, columnStart) + new Vector3(0f, 0.5f, 0f) + new Vector3(0, 0, jumpDistance / 2f));
        this.downJumpPoint.transform.position = (this.gridManager.GetPosition(rowStart, columnStart) + new Vector3(0f, 0.5f, 0f) + new Vector3(0, 0, -jumpDistance / 2f));
        this.rightUp.transform.parent =
            this.rightUp.transform.parent =
            this.leftDown.transform.parent =
            this.rightJumpPoint.transform.parent =
            this.leftJumpPoint.transform.parent =
            this.upJumpPoint.transform.parent =
            this.downJumpPoint.transform.parent =
            center.transform;
        SetColor();

        // set keys
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
        //Up Movement
        if (jumpInput && moveInput && Input.GetKey(this.keys["jump"]) && Input.GetKey(this.keys["up"]))
        {
            currRow -= jumpDistance;
            StartCoroutine(jump(upJumpPoint, rightUp, Vector3.right, Vector3.left));
            jumpInput = false;
            moveInput = false;
        }
        if (moveInput && Input.GetKey(this.keys["up"]))
        {
            currRow -= stepDistance;
            StartCoroutine(move(rightUp, Vector3.right));
            moveInput = false;
        }
        //Down Movement
        if (jumpInput && moveInput && Input.GetKey(this.keys["jump"]) && Input.GetKey(this.keys["down"]))
        {
            currRow += jumpDistance;
            StartCoroutine(jump(downJumpPoint, leftDown, Vector3.left, Vector3.right));
            jumpInput = false;
            moveInput = false;
        }
        if (moveInput && Input.GetKey(this.keys["down"]))
        {
            currRow += stepDistance;
            StartCoroutine(move(leftDown, Vector3.left));
            moveInput = false;
        }
        //Left Movement
        if (jumpInput && moveInput && Input.GetKey(this.keys["jump"]) && Input.GetKey(this.keys["left"]))
        {
            currColumn -= jumpDistance;
            StartCoroutine(jump(leftJumpPoint, leftDown, Vector3.forward, Vector3.back));
            jumpInput = false;
            moveInput = false;
        }
        if (moveInput && Input.GetKey(this.keys["left"]))
        {
            currColumn -= stepDistance;
            StartCoroutine(move(leftDown, Vector3.forward));
            moveInput = false;
        }
        //Right Movement
        if (jumpInput && moveInput && Input.GetKey(this.keys["jump"]) && Input.GetKey(this.keys["right"]))
        {
            currColumn += jumpDistance;
            StartCoroutine(jump(rightJumpPoint, rightUp, Vector3.back, Vector3.forward));
            jumpInput = false;
            moveInput = false;
        }
        if (moveInput && Input.GetKey(this.keys["right"]))
        {
            currColumn += stepDistance;
            StartCoroutine(move(rightUp, Vector3.back));
            moveInput = false;
        }
    }

    private void SetColor()
    {
        gridManager.SetColor(currRow, currColumn, Color.cyan);
    }

    //Generalized Move method *******************************************
    IEnumerator move(GameObject point, Vector3 direction)
    {
        for (int i = 0; i < (90 / degreesInStep); i ++)
        {
            player.transform.RotateAround(point.transform.position, direction, degreesInStep);
            yield return new WaitForSeconds(speed);
        }
        center.transform.position = player.transform.position;
        moveInput = true;
        SetColor();
    }

    IEnumerator jump(GameObject jumpPoint, GameObject point, Vector3 direction, Vector3 wobbleDirection)
    {
        for (int i =0; i < (180 / degreesInStep); i++)
        {
            player.transform.RotateAround(jumpPoint.transform.position, direction, degreesInStep);
            jumpCenter.transform.position = player.transform.position;
            player.transform.RotateAround(jumpCenter.transform.position, direction, degreesInStep * jumpRotationSpeed);
            yield return new WaitForSeconds(speed);
        }
        for (int i = 0; i < (jumpWobbleDegrees / degreesInStep); i++)
        {
            player.transform.RotateAround(point.transform.position, direction, degreesInStep);
            yield return new WaitForSeconds(speed * 2);
        }
        for (int i = 0; i < (jumpWobbleDegrees / degreesInStep); i++)
        {
            player.transform.RotateAround(point.transform.position, wobbleDirection, degreesInStep);
            yield return new WaitForSeconds(speed * 2);
        }
        center.transform.position = player.transform.position;
        yield return new WaitForSeconds(speed * jumpDelay);
        jumpInput = true;
        moveInput = true;
        SetColor();
    }

}
