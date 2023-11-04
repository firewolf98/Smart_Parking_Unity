using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movimenti : MonoBehaviour
{
    float moveSpeed=0.5F;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.UpArrow))
            transform.Translate(Vector3.forward*moveSpeed*Time.deltaTime);
        if(Input.GetKey(KeyCode.DownArrow))
            transform.Translate(-Vector3.forward*moveSpeed*Time.deltaTime);
        if(Input.GetKey(KeyCode.LeftArrow))
            transform.Translate(Vector3.left*moveSpeed*Time.deltaTime);
        if(Input.GetKey(KeyCode.RightArrow))
            transform.Translate(-Vector3.left*moveSpeed*Time.deltaTime);
        if(Input.GetKey(KeyCode.Space))
            transform.Translate(Vector3.up*moveSpeed*Time.deltaTime);
        if(Input.GetKey(KeyCode.LeftShift))
            transform.Translate(-Vector3.up*moveSpeed*Time.deltaTime);

    }
}
