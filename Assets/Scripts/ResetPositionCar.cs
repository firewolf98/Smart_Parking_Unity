using System.Collections.Generic;
using UnityEngine;

public class ResetPositionCar : MonoBehaviour
{
    // Punto di partenza
    private Vector3 startPosition = new Vector3(20.1f, 4.07f, 56.3f);
    private Quaternion startRotation = Quaternion.Euler(0, 270, 0);

    // Lista dei tag validi per il reset
    public List<string> resetTags;

    private void OnCollisionEnter(Collision collision)
    {
        // Verifica se il tag dell'oggetto colliso è nella lista dei tag validi
        if (resetTags.Contains(collision.gameObject.tag))
        {
            Debug.Log("Reset posizione a causa della collisione con: " + collision.gameObject.name);

            // Resetta posizione e rotazione
            transform.position = startPosition;
            transform.rotation = startRotation;
        }
    }
}

