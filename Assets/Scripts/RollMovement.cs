using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollMovement : MonoBehaviour
{
    private Vector3 offset;

    public GameObject player;

    public GameObject center;

    public GameObject rightUp;
    public GameObject leftDown;

    public GameObject rightJumpPoint;

    public int step = 9;

    public float speed = 0.01f;

    bool moveInput = true;
    bool jumpInput = true;

    // Update is called once per frame
    void Update()
    {
        if (moveInput && Input.GetKey(KeyCode.UpArrow))
        {
            StartCoroutine("moveUp");
            moveInput = false;
        }
        if (moveInput && Input.GetKey(KeyCode.DownArrow))
        {
            StartCoroutine("moveDown");
            moveInput = false;
        }
        if (moveInput && Input.GetKey(KeyCode.LeftArrow))
        {
            StartCoroutine("moveLeft");
            moveInput = false;
        }
        if (jumpInput && moveInput && Input.GetKey(KeyCode.Space))
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


    IEnumerator moveUp()
    {
        for (int i = 0; i < (90 / step); i++)
        {
            player.transform.RotateAround(rightUp.transform.position, Vector3.right, step);
            yield return new WaitForSeconds(speed);
        }
        center.transform.position = player.transform.position;
        moveInput = true;
    }

    IEnumerator moveDown()
    {
        for (int i = 0; i < (90 / step); i++)
        {
            player.transform.RotateAround(leftDown.transform.position, Vector3.left, step);
            yield return new WaitForSeconds(speed);
        }
        center.transform.position = player.transform.position;
        moveInput = true;
    }

    IEnumerator moveLeft()
    {
        for (int i = 0; i < (90 / step); i++)
        {
            player.transform.RotateAround(leftDown.transform.position, Vector3.forward, step);
            yield return new WaitForSeconds(speed);
        }
        center.transform.position = player.transform.position;
        moveInput = true;
    }

    IEnumerator moveRight()
    {
        for (int i = 0; i < (90 / step); i++)
        {
            player.transform.RotateAround(rightUp.transform.position, Vector3.back, step);
            yield return new WaitForSeconds(speed);
        }
        center.transform.position = player.transform.position;
        moveInput = true;
    }

    IEnumerator jumpRight()
    {
        for (int i = 0; i < (180 / step); i++)
        {
            player.transform.RotateAround(rightJumpPoint.transform.position, Vector3.back, step);
            yield return new WaitForSeconds(speed);
        }
        center.transform.position = player.transform.position;
        yield return new WaitForSeconds(speed*100);
        jumpInput = true;
        moveInput = true;
    }

}
