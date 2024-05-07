using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class TowerRange : MonoBehaviour
    {
        public Tower ParentTower;

        public void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Enemy"))
            {
                // Debug.Log("Enemy entered range: " + other.name);
                ParentTower.EnemyEnteredRange(other.gameObject);
            }
        }

        public void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Enemy"))
            {
                // Debug.Log("Enemy exited range: " + other.name);
                ParentTower.EnemyExitedRange(other.gameObject);
            }
        }
    }
}
