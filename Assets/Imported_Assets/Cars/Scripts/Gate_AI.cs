using UnityEngine;

public class Gate_AI : MonoBehaviour
{
    public GameObject bar;
    public float timeToClose = 5f; // Tempo di chiusura dopo che la macchina non è più rilevata
    private float rayLength = 5f;
    private bool isOpen = false;
    private float timeSinceLastDetection = 0f;

    private Quaternion startRotation;
    private Quaternion finalRotation = Quaternion.Euler(-180f, 90f, 0f);

    void Start()
    {
        startRotation = bar.transform.rotation;
    }

    void Update()
    {
        Quaternion rotation90Degrees = Quaternion.Euler(0f, 90f, 0f);
        Vector3 rotatedDirection = rotation90Degrees * transform.forward;

        RaycastHit hit;

        if (Physics.Raycast(transform.position, rotatedDirection, out hit, rayLength))
        {
            Debug.DrawRay(transform.position, rotatedDirection * rayLength, Color.red);

            Debug.Log("Il raycast ha colpito l'oggetto con il tag: " + hit.collider.tag);
            if (hit.collider.CompareTag("Car") && !isOpen)
            {
                OpenBar();
            }
        }
        else
        {
            Debug.DrawRay(transform.position, rotatedDirection * rayLength, Color.green);
            timeSinceLastDetection += Time.deltaTime;
            if (isOpen && timeSinceLastDetection >= timeToClose)
            {
                CloseBar();
            }
        }
    }

    void OpenBar()
    {
        bar.transform.rotation = finalRotation;
        isOpen = true;
        timeSinceLastDetection = 0f; // Resetta il tempo trascorso dall'ultima rilevazione
    }

    void CloseBar()
    {
        bar.transform.rotation = startRotation;
        isOpen = false;
    }
}
