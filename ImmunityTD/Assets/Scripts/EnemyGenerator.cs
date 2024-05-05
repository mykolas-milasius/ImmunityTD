using UnityEngine;
using System.Linq;
using System;

public class EnemyGenerator : MonoBehaviour
{
    /// <summary>
    /// All enemy prefabs
    /// </summary>
    public GameObject[] enemyPrefabs;

    /// <summary>
    /// Waypoints for enemies to follow
    /// </summary>
    public GameObject wayPoints;

    /// <summary>
    /// Speed of spawning enemies
    /// </summary>
    public int spawnSpeed;

    /// <summary>
    /// Interval for spawning enemies
    /// Random value between 0 and 10 divided by spawnSpeed
    /// </summary>
    private float spawnInterval;

    /// <summary>
    /// Timer for spawning enemies
    /// </summary>
    private float timer = 0f;

    /// <summary>
    /// Maximum number of enemies on the map
    /// </summary>
    public int enemyLimit = 100;

    /// <summary>
    /// Current number of enemies on the map
    /// </summary>
    public static int enemyCount = 0;
    
    /// <summary>
    /// Difficulty of the game
    /// 1 - easy, 2 - medium, 3 - hard, 4 - slavery
    /// </summary>
    public int difficulty = 2;

    /// <summary>
    /// Maximum number of enemies in the wave
    /// </summary>
    private int[] maxEnemiesInWave = new int[] { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 };

    /// <summary>
    /// Current number of enemies in the wave
    /// </summary>
    private int enemiesInWave = 0;

    /// <summary>
    /// Maximum number of waves
    /// </summary>
    private int maxWaves = 1;

    /// <summary>
    /// Current wave
    /// </summary>
    private int currentWave = 1;

    /// <summary>
    /// Start method for EnemyGenerator
    /// </summary>
    public void Start()
    {
        SetInitialValuesBasedOnDifficulty(difficulty);
        RandomInterval();
    }

    /// <summary>
    /// Update method for EnemyGenerator
    /// </summary>
    public void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0f;
            if (currentWave <= maxWaves)
            {
                if (enemyCount < enemyLimit && enemiesInWave < maxEnemiesInWave[currentWave - 1])
                {
                    SpawnEnemy();
                    Debug.Log(
                        "Difficulty: " + difficulty + ", " + GetDifficultyText() + ". " +
                        "Wave: " + currentWave + "/" + maxWaves + ". " +
                        "Enemies: " + enemiesInWave + "/" + maxEnemiesInWave[currentWave - 1]
                    );
                }
                else if (enemiesInWave >= maxEnemiesInWave[currentWave - 1])
                {
                    CheckWaveCompletion();
                }
            }   
        }   
    }

    /// <summary>
    /// Random interval for spawning enemies
    /// </summary>
    void RandomInterval()
    {
        spawnInterval = UnityEngine.Random.Range(0.1f, 10f / spawnSpeed);
    }

    /// <summary>
    /// Spawning enemies
    /// </summary>
    public void SpawnEnemy()
    {
        int randomIndex = UnityEngine.Random.Range(0, enemyPrefabs.Length);
        GameObject newEnemy = Instantiate(enemyPrefabs[randomIndex], transform.position, Quaternion.identity);
        Enemy enemyScript = newEnemy.GetComponent<Enemy>();
        EnemyPath enemyPathScript = newEnemy.GetComponent<EnemyPath>();
        enemyPathScript.waypoints = wayPoints.GetComponentsInChildren<Transform>().Skip(1).ToArray();

        if (wayPoints != null)
        {
            enemyPathScript.waypoints = wayPoints.GetComponentsInChildren<Transform>().Skip(1).ToArray();
        }

        if (enemyScript != null && enemyPathScript != null)
        {
            enemyPathScript.SetSpeed(enemyScript.speed);
        }

        enemyCount++;
        enemiesInWave++;
        RandomInterval();

        // Debug.Log("enemies: " + enemyCount + "/" + enemyLimit);
    }

    /// <summary>
    /// Sets initial values based on the given difficulty
    /// </summary>
    /// <param name="diff">Difficulty</param>
    void SetInitialValuesBasedOnDifficulty(int diff)
    {
        switch (diff)
        {
            case 1:
                maxWaves = 2;
                spawnSpeed = 1;
                break;
            case 2:
                maxWaves = 3;
                spawnSpeed = 3;
                break;
            case 3:
                maxWaves = 4;
                spawnSpeed = 5;
                break;
            case 4:
                maxWaves = 5;
                spawnSpeed = 10;
                break;
            default:
                maxWaves = 1;
                spawnSpeed = 3;
                break;
        }
    }

    /// <summary>
    /// Checks if the wave is completed.
    /// If so, starts the next wave or ends the game.
    /// </summary>
    void CheckWaveCompletion()
    {
        if (GameObject.FindObjectsOfType<Enemy>().Length == 0)
        {
            currentWave++;
            if (currentWave > maxWaves)
            {
                Debug.Log("All waves completed");
            }
            else
            {
                enemiesInWave = 0;
                Debug.Log("Wave " + currentWave + " starting now");
            }
        }
    }

    string GetDifficultyText()
    {
        switch (difficulty)
        {
            case 1:
                return "Easy";
            case 2:
                return "Medium";
            case 3:
                return "Hard";
            case 4:
                return "Slavery";
        }
        
        return "bbz";
    }
}
