using UnityEngine;
using System.Linq;
using System;

public class EnemyGenerator : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject[] enemyArray;
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
        spawnInterval = UnityEngine.Random.Range(0, 10f / spawnSpeed);
    }

    public void SpawnEnemy()
    {
        float random = UnityEngine.Random.Range(0, enemyArray.Length);
        GameObject newEnemy = Instantiate(enemyArray[(int)Math.Round(random, 0)], transform.position, Quaternion.identity);
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
