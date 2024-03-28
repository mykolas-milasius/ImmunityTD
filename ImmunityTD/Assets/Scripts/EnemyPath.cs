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
        Vector2 targetPos = waypoints[currentWaypointIndex].position / 100; // Scale down by 100
        transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, targetPos) < 0.01f)
        {
            currentWaypointIndex++;
        }
    }
        else
        {
            // Enemy reached the end of the path
            // Implement what happens next (e.g., recycle enemy, reduce player life, etc.)
        }
    }
}
