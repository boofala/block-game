using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMovement : MonoBehaviour
{
    //[SerializeField]
    public float cube1Speed = 1;
    // Start is called before the first frame update
    //void Start()
    //{
        
    //}

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 cubeMovement = new Vector3(horizontalInput, verticalInput);

        transform.position += cubeMovement * Time.deltaTime * cube1Speed;
    }
}
