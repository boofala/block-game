using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollMovement : MonoBehaviour
{
    private Vector3 offset;

    public GameObject player;

    public GameObject center;
    public GameObject jumpCenter;

    public GameObject rightUp;
    public GameObject leftDown;

    public GameObject rightJumpPoint;
    public GameObject leftJumpPoint;
    public GameObject upJumpPoint;
    public GameObject downJumpPoint;

    public int jumpDelay = 0;
    public int jumpRotationSpeed = 2;

    public int degreesInStep = 9;

    public float speed = 0.01f;

    bool moveInput = true;
    bool jumpInput = true;

    // Update is called once per frame
    void Update()
    {
        //Up Movement
        if (jumpInput && moveInput && Input.GetKey(KeyCode.Space) && Input.GetKey(KeyCode.UpArrow))
        {
            StartCoroutine("jumpUp");
            jumpInput = false;
            moveInput = false;
        }
        if (moveInput && Input.GetKey(KeyCode.UpArrow))
        {
            StartCoroutine("moveUp");
            moveInput = false;
        }
        //Down Movement
        if (jumpInput && moveInput && Input.GetKey(KeyCode.Space) && Input.GetKey(KeyCode.DownArrow))
        {
            StartCoroutine("jumpDown");
            jumpInput = false;
            moveInput = false;
        }
        if (moveInput && Input.GetKey(KeyCode.DownArrow))
        {
            StartCoroutine("moveDown");
            moveInput = false;
        }
        //Left Movement
        if (jumpInput && moveInput && Input.GetKey(KeyCode.Space) && Input.GetKey(KeyCode.LeftArrow))
        {
            StartCoroutine("jumpLeft");
            jumpInput = false;
            moveInput = false;
        }
        if (moveInput && Input.GetKey(KeyCode.LeftArrow))
        {
            StartCoroutine("moveLeft");
            moveInput = false;
        }
        //Right Movement
        if (jumpInput && moveInput && Input.GetKey(KeyCode.Space) && Input.GetKey(KeyCode.RightArrow))
        {
            StartCoroutine("jumpRight");
            jumpInput = false;
            moveInput = false;
        }
        if (moveInput && Input.GetKey(KeyCode.RightArrow))
        {
            StartCoroutine("moveRight");
            moveInput = false;
        }
               
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
        center.transform.position = player.transform.position;
        yield return new WaitForSeconds(speed * jumpDelay);
        jumpInput = true;
        moveInput = true;
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
        center.transform.position = player.transform.position;
        yield return new WaitForSeconds(speed * jumpDelay);
        jumpInput = true;
        moveInput = true;
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
        yield return new WaitForSeconds(speed * jumpDelay);
        jumpInput = true;
        moveInput = true;
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
        yield return new WaitForSeconds(speed* jumpDelay);
        jumpInput = true;
        moveInput = true;
    }

}
