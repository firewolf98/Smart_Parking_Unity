using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movimenti : MonoBehaviour
{
    public List<Transform> waypoints;
    public float moveSpeed = 4.5f;
    public float rotationSpeed = 9.5f;
    private int currentWaypointIndex = 0;

    void Update()
    {
        if (waypoints.Count == 0) return;

        Transform targetWaypoint = waypoints[currentWaypointIndex];
        Vector3 directionToTarget = targetWaypoint.position - transform.position;
        directionToTarget.y = 0; // Ignora la componente y per mantenere la macchina sul piano orizzontale

        // Muovi verso il waypoint
        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, moveSpeed * Time.deltaTime);

        // Ruota verso il waypoint
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // Se la macchina è vicina al waypoint, passa al successivo
        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.5f)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Count)
            {
                currentWaypointIndex = 0; // Ricomincia dal primo waypoint o fermati qui a seconda della tua logica
            }
        }
    }
}
