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
            SpawnEnemy();
        }   
    }

    void RandomInterval()
    {
        spawnInterval = Random.Range(0.1f, 3f);
    }

    void SpawnEnemy(){
        GameObject newEnemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        Enemy enemyScript = newEnemy.GetComponent<Enemy>();
        EnemyPath enemyPathScript = newEnemy.GetComponent<EnemyPath>();
        enemyPathScript.waypoints = wayPoints.GetComponentsInChildren<Transform>().Skip(1).ToArray();

        if (enemyScript != null && enemyPathScript != null)
        {
            enemyPathScript.SetSpeed(enemyScript.speed);
        }
        RandomInterval();
    }
}
