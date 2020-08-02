﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollMovement : MonoBehaviour
{
    private Vector3 offset;

    public GameObject player;

    public GameObject center;

    public GameObject rightUp;
    public GameObject leftDown;

    public int step = 9;

    public float speed = 0.01f;

    bool input = true;

    // Update is called once per frame
    void Update()
    {
        if (input && Input.GetKey(KeyCode.UpArrow))
        {
            StartCoroutine("moveUp");
            input = false;
        }
        if (input && Input.GetKey(KeyCode.DownArrow))
        {
            StartCoroutine("moveDown");
            input = false;
        }
        if (input && Input.GetKey(KeyCode.LeftArrow))
        {
            StartCoroutine("moveLeft");
            input = false;
        }
        if (input && Input.GetKey(KeyCode.RightArrow))
        {
            StartCoroutine("moveRight");
            input = false;
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
        input = true;
    }

    IEnumerator moveDown()
    {
        for (int i = 0; i < (90 / step); i++)
        {
            player.transform.RotateAround(leftDown.transform.position, Vector3.left, step);
            yield return new WaitForSeconds(speed);
        }
        center.transform.position = player.transform.position;
        input = true;
    }

    IEnumerator moveLeft()
    {
        for (int i = 0; i < (90 / step); i++)
        {
            player.transform.RotateAround(leftDown.transform.position, Vector3.forward, step);
            yield return new WaitForSeconds(speed);
        }
        center.transform.position = player.transform.position;
        input = true;
    }

    IEnumerator moveRight()
    {
        for (int i = 0; i < (90 / step); i++)
        {
            player.transform.RotateAround(rightUp.transform.position, Vector3.back, step);
            yield return new WaitForSeconds(speed);
        }
        center.transform.position = player.transform.position;
        input = true;
    }
}
