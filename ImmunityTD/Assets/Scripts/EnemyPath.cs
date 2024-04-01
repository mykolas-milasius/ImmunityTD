using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPath : MonoBehaviour
{
    public Transform[] waypoints;
    private int currentWaypointIndex = 0;
    private float speed = 1f; // Speed in Unity units per second
    public GameObject gameOverText;

    void Update()
    {
        MoveAlongPath();
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    void MoveAlongPath()
    {
        if (currentWaypointIndex < waypoints.Length)
        {
            Transform targetWaypoint = waypoints[currentWaypointIndex];

            // Calculate the adjusted position by subtracting 960 from x and 540 from y
            Vector2 adjustedPosition = new Vector2((targetWaypoint.position.x - 960) / 100, (targetWaypoint.position.y - 540) / 100);
            transform.position = Vector2.MoveTowards(transform.position, adjustedPosition, speed * Time.deltaTime);
            
            if ((Vector2)transform.position == adjustedPosition)
            {
                currentWaypointIndex++;
            }
        }
        else
        {
            Destroy(gameObject);
            // gameOverText.SetActive(true);
        }
    }
}
