using UnityEngine;

public class Gate_AI : MonoBehaviour
{
    public GameObject bar;
    public float timeRotation = 10f;
    private float timeSpent = 0f;
    private float timeSpentToLower = 0f;
    private Quaternion startRotation;
    private Quaternion finalRotation = Quaternion.Euler(-180f, 90f, 0f);
    private bool opened = false;
    private float rayLenght = 5f;

    void Start()
    {
        startRotation = bar.transform.rotation;
    }

    void Update()
    {
        Quaternion rotazione90Gradi = Quaternion.Euler(0f, 90f, 0f);
        Vector3 direzioneRuotata = rotazione90Gradi * transform.forward;

        RaycastHit hit;

        // Lancia un raycast nella direzione ruotata dalla posizione dell'oggetto corrente
        if (Physics.Raycast(transform.position, direzioneRuotata, out hit, rayLenght))
        {
            // Visualizza il raycast nel Scene View
            Debug.DrawRay(transform.position, direzioneRuotata * rayLenght, Color.red);

            // Esegui altre azioni in base alla collisione
            Debug.Log("Il raycast ha colpito l'oggetto con il tag: " + hit.collider.tag);
            if (hit.collider.CompareTag("Car"))
            {
                // Se l'oggetto davanti è stato colpito, avvia la rotazione
                raiseBar();
            }
        }
        else
        {
            // Se il raycast non colpisce nulla, puoi comunque visualizzarlo nel Scene View
            Debug.DrawRay(transform.position, direzioneRuotata * rayLenght, Color.green);
            if(opened)
            {
                lowerBar();
            }
        }
    }


    void raiseBar()
    {
        if (timeSpent < timeRotation)
        {
            // Aggiorna il tempo trascorso
            timeSpent += Time.deltaTime;

            // Calcola l'interpolazione tra la rotazione iniziale e la rotazione desiderata
            float t = timeSpent / timeRotation;
            bar.transform.rotation = Quaternion.Lerp(startRotation, finalRotation, t);
        }
        else
        {
            // Imposta la rotazione finale dopo il completamento della rotazione
            bar.transform.rotation = finalRotation;
        }
        opened = true;
    }

    void lowerBar()
    {
        if (timeSpentToLower < timeRotation)
        {
            // Aggiorna il tempo trascorso
            timeSpentToLower += Time.deltaTime;

            // Calcola l'interpolazione tra la rotazione iniziale e la rotazione desiderata
            float t = timeSpentToLower / timeRotation;
            bar.transform.rotation = Quaternion.Lerp(finalRotation, startRotation, t);
        }
        else
        {
            // Imposta la rotazione finale dopo il completamento della rotazione
            bar.transform.rotation = startRotation;
        }
    }
}
