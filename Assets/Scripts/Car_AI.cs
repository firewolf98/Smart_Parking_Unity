using UnityEngine;

public class Car_AI : MonoBehaviour
{
    public float distance = 3f;
    public float carSpeed = 5f;

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
}
