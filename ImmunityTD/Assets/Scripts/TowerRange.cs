using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerRange : MonoBehaviour
{
    public Tower parentTower;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Debug.Log("Enemy entered range: " + other.name);
            parentTower.EnemyEnteredRange(other.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Debug.Log("Enemy exited range: " + other.name);
            parentTower.EnemyExitedRange(other.gameObject);
        }
    }
}