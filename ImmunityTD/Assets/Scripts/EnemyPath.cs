using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPath : MonoBehaviour
{
    public Transform[] waypoints;
    private int currentWaypointIndex = 0;
    public float speed = 1.0f; // Speed in Unity units per second

    void Update()
    {
        MoveAlongPath();
    }

    void MoveAlongPath()
    {
        if (currentWaypointIndex < waypoints.Length)
        {
            Transform targetWaypoint = waypoints[currentWaypointIndex];
            // Calculate the distance to the next waypoint
            float distance = Vector2.Distance(transform.position, targetWaypoint.position);
            // Adjust the speed based on the distance and PPU
            float adjustedSpeed = (speed / 100) * (distance / 100);
            // Use the adjusted speed for movement
            transform.position = Vector2.MoveTowards(transform.position, targetWaypoint.position, adjustedSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, targetWaypoint.position) < 0.01f) // A small threshold for reaching the waypoint
            {
                currentWaypointIndex++;
            }
        }
        else
        {
            // Handle what happens when the enemy reaches the end of the path
        }
    }
}
