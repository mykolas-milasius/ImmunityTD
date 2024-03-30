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
            parentTower.EnemyEnteredRange(other.gameObject);
            Debug.Log("added " + other.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            parentTower.EnemyExitedRange(other.gameObject);
            Debug.Log("removed " + other.gameObject);
        }
    }
}
