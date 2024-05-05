using UnityEngine;
using System.Linq;
using System;
using TMPro;

public class EnemyGenerator : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public GameObject wayPoints;
    public int spawnSpeed;

    /// <summary>
    /// Interval for spawning enemies
    /// Random value between 0 and 10 divided by spawnSpeed
    /// </summary>
    private float spawnInterval;
    private float timer = 0f;
    public int enemyLimit = 100;
    public static int enemyCount = 0;
    
    /// <summary>
    /// Difficulty of the game
    /// 1 - easy, 2 - medium, 3 - hard, 4 - slavery
    /// </summary>
    public int difficulty = 2;
    private int[] maxEnemiesInWave = new int[] { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 };
    private int enemiesInWave = 0;
    private int maxWaves = 1;
    private int currentWave = 1;

    public TextMeshProUGUI enemyWaveText;

    public void Start()
    {
        SetInitialValuesBasedOnDifficulty(difficulty);
        RandomInterval();
    }

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
                    Debug.Log($"Difficulty: {difficulty}, {GetDifficultyText()}. Wave: {currentWave}/{maxWaves}. Enemies: {enemiesInWave}/{maxEnemiesInWave[currentWave - 1]}");
                }
                else if (enemiesInWave >= maxEnemiesInWave[currentWave - 1])
                {
                    CheckWaveCompletion();
                }
            }   
        }   
    }

    private void RandomInterval()
    {
        spawnInterval = UnityEngine.Random.Range(0.1f, 10f / spawnSpeed);
    }

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

    private void SetInitialValuesBasedOnDifficulty(int diff)
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
    private void CheckWaveCompletion()
    {
        if (GameObject.FindObjectsOfType<Enemy>().Length == 0)
        {
            currentWave++;
            Player.Instance.TurnOffGame();

            if (currentWave > maxWaves)
            {
                Debug.Log("All waves completed");
            }
            else
            {
                enemiesInWave = 0;
                UpdateEnemyWaveText();
                Debug.Log($"Wave: {currentWave}/{maxWaves} starting now");
            }
        }
    }

    private string GetDifficultyText()
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
    
    private void UpdateEnemyWaveText()
    {
        Player.Instance.UpdateEnemyWaveText(currentWave, maxWaves, maxEnemiesInWave[currentWave - 1]);
    }
}
