using UnityEngine;

public class Car_AI : MonoBehaviour
{
    public float distance = 3f;
    public float carSpeed = 5f;
    public bool findParking = true;
    private Rigidbody rb;
    private bool isLookingForPark;
    private bool isParking;
    private Vector3 firstPosition;
    private Quaternion firstRotation;
    private Vector3 finalPosition;
    private int steps = 0;
    private void Update()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, transform.forward, out hit, distance);

        Vector3 posizioneOrigine = transform.position;
        Vector3 direzione = transform.forward;

        if (Physics.Raycast(posizioneOrigine, direzione, out hit, distance))
        {
            Debug.DrawRay(posizioneOrigine, direzione * distance, Color.red);
            Debug.Log("Il raycast ha colpito l'oggetto con il tag: " + hit.collider.tag);
        }
        else
        {
            Debug.DrawRay(posizioneOrigine, direzione * distance, Color.green);
        }

        if (Physics.Raycast(posizioneOrigine, direzione * distance, out hit, distance))
        {
            if (hit.collider.CompareTag("Grass"))
            {
                Stop();
            }
            else
            {
                //Move();
            }
        }
        else
        {
            //Move();
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

    private void Reset()
    {
        if (findParking)
        {
            isLookingForPark = true;
            isParking = false;
        }

        rb.transform.position = firstPosition;
        rb.transform.rotation = firstRotation;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        steps = 0;
    }
}
