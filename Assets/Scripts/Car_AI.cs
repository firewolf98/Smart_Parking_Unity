using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents.Integrations.Match3;
using UnityEngine;

public class Car_AI : MonoBehaviour
{
    public float distance = 2f;
    public float carSpeed = 5f;

    private void Update()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, transform.forward, out hit, distance);

        if(hit.transform)
        {
            if(hit.transform.tag == "Gate")
            {
                Stop();
            }
        }
        else
        {
            Move();
        }
    }

    void Stop()
    {
        transform.position += new Vector3(0, 0, 0);
    }

    void Move()
    {
        transform.position -= new Vector3(carSpeed * Time.deltaTime, 0, 0);
    }
}
