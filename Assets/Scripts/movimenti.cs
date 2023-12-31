using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movimenti : MonoBehaviour
{
    float moveSpeed=4.5F;
    float rotationSpeed = 9.5F;

    Vector3 forward = new Vector3(0, 0, 1);
    Vector3 backward = new Vector3(0, 0, -1);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.UpArrow))
            transform.Translate(forward*moveSpeed*Time.deltaTime);
        if(Input.GetKey(KeyCode.DownArrow))
            transform.Translate(backward*moveSpeed*Time.deltaTime);
        if(Input.GetKey(KeyCode.LeftArrow))
            transform.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime);
        if (Input.GetKey(KeyCode.RightArrow))
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        if (Input.GetKey(KeyCode.Space))
            transform.Translate(Vector3.up*moveSpeed*Time.deltaTime);
        if(Input.GetKey(KeyCode.LeftShift))
            transform.Translate(-Vector3.up*moveSpeed*Time.deltaTime);
    }
}
