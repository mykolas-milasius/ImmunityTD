using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class EnemyPath : MonoBehaviour
    {
        public Transform[] Waypoints;

        private int _currentWaypointIndex = 0;
        private float _speed = 1f; // Speed in Unity units per second
        private Enemy _enemyComponent;

        private void Start()
        {
            _enemyComponent = GetComponent<Enemy>();
        }

        public void Update()
        {
            MoveAlongPath();
        }

        public void SetSpeed(float newSpeed)
        {
            _speed = newSpeed;
        }

        void MoveAlongPath()
        {
            if (_currentWaypointIndex < Waypoints.Length)
            {
                _speed = _enemyComponent.GetSpeed();
                Transform targetWaypoint = Waypoints[_currentWaypointIndex];
                Vector2 targetPosition = new Vector2(targetWaypoint.position.x, targetWaypoint.position.y);
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, _speed * Time.deltaTime);

                if ((Vector2)transform.position == targetPosition)
                {
                    _currentWaypointIndex++;
                }
            }
            else
            {
                Player.TakeDamage(_enemyComponent.GetDamageIfNotKilled());
                Destroy(gameObject);
                EnemyGenerator.EnemyCount--;
                // gameOverText.SetActive(true);
            }
        }
    }
}
