using UnityEngine;
using System.Linq;
using System;
using TMPro;

public class EnemyGenerator : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public GameObject wayPoints;
    public int spawnSpeed;
    public int difficulty = 2; // 1 - easy, 2 - medium, 3 - hard, 4 - slavery
    public int enemyLimit = 100;
    
    private float spawnInterval; // Random value between 0 and 10 divided by spawnSpeed
    private float timer = 0f;
    public static int enemyCount = 0;
    private int[] maxEnemiesInWave = new int[] { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 };
    private int enemiesInWave = 0;
    private int maxWaves = 1;
    private int currentWave = 1;
    
    public void Start()
    {
        SetInitialValuesBasedOnDifficulty(difficulty);
        RandomInterval();
        Player.Instance.UpdateEnemyWaveText(currentWave, maxWaves, maxEnemiesInWave[currentWave - 1]);
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
