using UnityEngine;
using System.Linq;

public class EnemyGenerator : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject wayPoints;
    public float spawnInterval;
    private float timer = 0f;

    void Start()
    {
        RandomInterval();
    }

    void Update()
    {
        // Update the timer every frame
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            timer = 0f;
            
            GameObject newEnemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            EnemyPath enemyScript = newEnemy.GetComponent<EnemyPath>();
            enemyScript.waypoints = wayPoints.GetComponentsInChildren<Transform>().Skip(1).ToArray();

            RandomInterval();
        }
    }

    void RandomInterval()
    {
        spawnInterval = Random.Range(0.1f, 3f);
    }
}
