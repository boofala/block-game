using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollMovement : MonoBehaviour
{
    public int rowStart, columnStart;

    public GridManager gridManager;

    public GameObject player;

    public GameObject center;
    public GameObject jumpCenter;

    public GameObject rightUp;
    public GameObject leftDown;

    public GameObject rightJumpPoint;
    public GameObject leftJumpPoint;
    public GameObject upJumpPoint;
    public GameObject downJumpPoint;

    public bool isWasd;

    public Vector3 direction;
    public int jumpDelay = 0;
    public int jumpRotationSpeed = 2;
    public int jumpWobbleDegrees = 5;

    public int degreesInStep = 9;

    public float speed = 0.01f;

    private int currRow, currColumn;

    private int stepDistance = 1;
    private int jumpDistance = 4;

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
            StartCoroutine("jumpUp");
            jumpInput = false;
            moveInput = false;
        }
        if (moveInput && Input.GetKey(this.keys["up"]))
        {
            currRow -= stepDistance;
            direction = Vector3.right;
            StartCoroutine(move(rightUp, direction));
            moveInput = false;
        }
        //Down Movement
        if (jumpInput && moveInput && Input.GetKey(this.keys["jump"]) && Input.GetKey(this.keys["down"]))
        {
            currRow += jumpDistance;
            StartCoroutine("jumpDown");
            jumpInput = false;
            moveInput = false;
        }
        if (moveInput && Input.GetKey(this.keys["down"]))
        {
            currRow += stepDistance;
            direction = Vector3.left;
            StartCoroutine(move(leftDown, direction));
            moveInput = false;
        }
        //Left Movement
        if (jumpInput && moveInput && Input.GetKey(this.keys["jump"]) && Input.GetKey(this.keys["left"]))
        {
            currColumn -= jumpDistance;
            StartCoroutine("jumpLeft");
            jumpInput = false;
            moveInput = false;
        }
        if (moveInput && Input.GetKey(this.keys["left"]))
        {
            currColumn -= stepDistance;
            direction = Vector3.forward;
            StartCoroutine(move(leftDown, direction));
            moveInput = false;
        }
        //Right Movement
        if (jumpInput && moveInput && Input.GetKey(this.keys["jump"]) && Input.GetKey(this.keys["right"]))
        {
            currColumn += jumpDistance;
            StartCoroutine("jumpRight");
            jumpInput = false;
            moveInput = false;
        }
        if (moveInput && Input.GetKey(this.keys["right"]))
        {
            currColumn += stepDistance;
            direction = Vector3.back;
            StartCoroutine(move(rightUp, direction));
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

    //Up Movement
    IEnumerator moveUp()
    {
        for (int i = 0; i < (90 / degreesInStep); i++)
        {
            player.transform.RotateAround(rightUp.transform.position, Vector3.right, degreesInStep);
            yield return new WaitForSeconds(speed);
        }
        center.transform.position = player.transform.position;
        moveInput = true;
        SetColor();
    }

    IEnumerator jumpUp()
    {
        for (int i = 0; i < (180 / degreesInStep); i++)
        {
            player.transform.RotateAround(upJumpPoint.transform.position, Vector3.right, degreesInStep);
            jumpCenter.transform.position = player.transform.position;
            player.transform.RotateAround(jumpCenter.transform.position, Vector3.right, degreesInStep*jumpRotationSpeed);
            yield return new WaitForSeconds(speed);
        }
        for (int i = 0; i < (jumpWobbleDegrees / degreesInStep); i++)
        {
            player.transform.RotateAround(rightUp.transform.position, Vector3.right, degreesInStep);
            yield return new WaitForSeconds(speed * 2);
        }
        for (int i = 0; i < (jumpWobbleDegrees / degreesInStep); i++)
        {
            player.transform.RotateAround(rightUp.transform.position, Vector3.left, degreesInStep);
            yield return new WaitForSeconds(speed * 2);
        }
        center.transform.position = player.transform.position;
        yield return new WaitForSeconds(speed * jumpDelay);
        jumpInput = true;
        moveInput = true;
        SetColor();
    }

    //Down Movement
    IEnumerator moveDown()
    {
        for (int i = 0; i < (90 / degreesInStep); i++)
        {
            player.transform.RotateAround(leftDown.transform.position, Vector3.left, degreesInStep);
            yield return new WaitForSeconds(speed);
        }
        center.transform.position = player.transform.position;
        moveInput = true;
        SetColor();
    }

    IEnumerator jumpDown()
    {
        for (int i = 0; i < (180 / degreesInStep); i++)
        {
            player.transform.RotateAround(downJumpPoint.transform.position, Vector3.left, degreesInStep);
            jumpCenter.transform.position = player.transform.position;
            player.transform.RotateAround(jumpCenter.transform.position, Vector3.left, degreesInStep * jumpRotationSpeed);
            yield return new WaitForSeconds(speed);
        }
        for (int i = 0; i < (jumpWobbleDegrees / degreesInStep); i++)
        {
            player.transform.RotateAround(rightUp.transform.position, Vector3.left, degreesInStep);
            yield return new WaitForSeconds(speed * 2);
        }
        for (int i = 0; i < (jumpWobbleDegrees / degreesInStep); i++)
        {
            player.transform.RotateAround(rightUp.transform.position, Vector3.right, degreesInStep);
            yield return new WaitForSeconds(speed * 2);
        }
        center.transform.position = player.transform.position;
        yield return new WaitForSeconds(speed * jumpDelay);
        jumpInput = true;
        moveInput = true;
        SetColor();
    }

    //Left Movement
    IEnumerator moveLeft()
    {
        for (int i = 0; i < (90 / degreesInStep); i++)
        {
            player.transform.RotateAround(leftDown.transform.position, Vector3.forward, degreesInStep);
            yield return new WaitForSeconds(speed);
        }
        center.transform.position = player.transform.position;
        moveInput = true;
        SetColor();
    }

    IEnumerator jumpLeft()
    {
        for (int i = 0; i < (180 / degreesInStep); i++)
        {
            player.transform.RotateAround(leftJumpPoint.transform.position, Vector3.forward, degreesInStep);
            jumpCenter.transform.position = player.transform.position;
            player.transform.RotateAround(jumpCenter.transform.position, Vector3.forward, degreesInStep * jumpRotationSpeed);
            yield return new WaitForSeconds(speed);
        }
        center.transform.position = player.transform.position;
        for (int i = 0; i < (jumpWobbleDegrees / degreesInStep); i++)
        {
            player.transform.RotateAround(rightUp.transform.position, Vector3.forward, degreesInStep);
            yield return new WaitForSeconds(speed * 2);
        }
        for (int i = 0; i < (jumpWobbleDegrees / degreesInStep); i++)
        {
            player.transform.RotateAround(rightUp.transform.position, Vector3.back, degreesInStep);
            yield return new WaitForSeconds(speed * 2);
        }
        center.transform.position = player.transform.position;
        yield return new WaitForSeconds(speed * jumpDelay);
        jumpInput = true;
        moveInput = true;
        SetColor();
    }

    //Right Movement
    IEnumerator moveRight()
    {
        for (int i = 0; i < (90 / degreesInStep); i++)
        {
            player.transform.RotateAround(rightUp.transform.position, Vector3.back, degreesInStep);
            yield return new WaitForSeconds(speed);
        }
        center.transform.position = player.transform.position;
        moveInput = true;
        SetColor();
    }

    IEnumerator jumpRight()
    {
        for (int i = 0; i < (180 / degreesInStep); i++)
        {
            player.transform.RotateAround(rightJumpPoint.transform.position, Vector3.back, degreesInStep);
            jumpCenter.transform.position = player.transform.position;
            player.transform.RotateAround(jumpCenter.transform.position, Vector3.back, degreesInStep * jumpRotationSpeed);
            yield return new WaitForSeconds(speed);
        }
        center.transform.position = player.transform.position;
        for (int i = 0; i < (jumpWobbleDegrees / degreesInStep); i++)
        {
            player.transform.RotateAround(rightUp.transform.position, Vector3.back, degreesInStep);
            yield return new WaitForSeconds(speed*2);
        }
        for (int i = 0; i < (jumpWobbleDegrees / degreesInStep); i++)
        {
            player.transform.RotateAround(rightUp.transform.position, Vector3.forward, degreesInStep);
            yield return new WaitForSeconds(speed*2);
        }
        center.transform.position = player.transform.position;
        yield return new WaitForSeconds(speed* jumpDelay);
        jumpInput = true;
        moveInput = true;
        SetColor();
    }

}
