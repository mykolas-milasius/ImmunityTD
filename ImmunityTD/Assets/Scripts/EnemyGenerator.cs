using UnityEngine;
using System.Linq;

public class EnemyGenerator : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject wayPoints;
    public int spawnSpeed = 3; // Max 100
    private float spawnInterval;
    private float timer = 0f;
    public int enemyLimit = 100;
    public static int enemyCount = 0;

    public void Start()
    {
        RandomInterval();
    }

    public void Update()
    {
        // Update the timer every frame
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0f;
            if (enemyCount < enemyLimit) 
            {
                SpawnEnemy();
            }
        }   
    }

    void RandomInterval()
    {
        spawnInterval = Random.Range(0, 10f / spawnSpeed);
    }

    public void SpawnEnemy()
    {
        GameObject newEnemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        Enemy enemyScript = newEnemy.GetComponent<Enemy>();
        EnemyPath enemyPathScript = newEnemy.GetComponent<EnemyPath>();
        enemyPathScript.waypoints = wayPoints.GetComponentsInChildren<Transform>().Skip(1).ToArray();

        if (enemyScript != null && enemyPathScript != null)
        {
            enemyPathScript.SetSpeed(enemyScript.speed);
        }

        enemyCount++;
        RandomInterval();

        // Debug.Log("enemies: " + enemyCount + "/" + enemyLimit);
    }
}
