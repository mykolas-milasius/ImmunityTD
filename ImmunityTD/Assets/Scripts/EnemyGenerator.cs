using UnityEngine;
using System.Linq;
using System;
using TMPro;

public class EnemyGenerator : MonoBehaviour
{
    public GameObject[] EnemyPrefabs;
    public GameObject WayPoints;
    public int SpawnSpeed;
    public int EnemyLimit = 100;
    public static int EnemyCount = 0;
    
    private float _spawnInterval; // Random value between 0 and 10 divided by spawnSpeed
    private float _timer = 0f;
    private int[] _maxEnemiesInWave = { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 };
    private int _enemiesInWave = 0;
    private int _maxWaves = 1;
    private int _currentWave = 1;
    
    public void Start()
    {
        SetInitialValuesBasedOnDifficulty(Player.Instance.Difficulty);
        RandomInterval();
        Player.Instance.UpdateEnemyWaveText(_currentWave, _maxWaves, _maxEnemiesInWave[_currentWave - 1]);
    }

    public void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= _spawnInterval)
        {
            _timer = 0f;
            if (_currentWave <= _maxWaves)
            {
                if (EnemyCount < EnemyLimit && _enemiesInWave < _maxEnemiesInWave[_currentWave - 1])
                {
                    SpawnEnemy();
                    Debug.Log($"Difficulty: {Player.Instance.Difficulty}, {GetDifficultyText()}. Wave: {_currentWave}/{_maxWaves}. Enemies: {_enemiesInWave}/{_maxEnemiesInWave[_currentWave - 1]}");
                }
                else if (_enemiesInWave >= _maxEnemiesInWave[_currentWave - 1])
                {
                    CheckWaveCompletion();
                }
            }   
        }   
    }

    private void RandomInterval()
    {
        _spawnInterval = UnityEngine.Random.Range(0.1f, 10f / SpawnSpeed);
    }

    public void SpawnEnemy()
    {
        int randomIndex = UnityEngine.Random.Range(0, EnemyPrefabs.Length);
        GameObject newEnemy = Instantiate(EnemyPrefabs[randomIndex], transform.position, Quaternion.identity);
        Enemy enemyScript = newEnemy.GetComponent<Enemy>();
        EnemyPath enemyPathScript = newEnemy.GetComponent<EnemyPath>();
        enemyPathScript.waypoints = WayPoints.GetComponentsInChildren<Transform>().Skip(1).ToArray();

        if (WayPoints != null)
        {
            enemyPathScript.waypoints = WayPoints.GetComponentsInChildren<Transform>().Skip(1).ToArray();
        }

        if (enemyScript != null && enemyPathScript != null)
        {
            enemyPathScript.SetSpeed(enemyScript.speed);
        }

        EnemyCount++;
        _enemiesInWave++;
        RandomInterval();

        // Debug.Log("enemies: " + enemyCount + "/" + enemyLimit);
    }

    private void SetInitialValuesBasedOnDifficulty(int diff)
    {
        switch (diff)
        {
            case 1:
                _maxWaves = 2;
                SpawnSpeed = 1;
                break;
            case 2:
                _maxWaves = 3;
                SpawnSpeed = 3;
                break;
            case 3:
                _maxWaves = 4;
                SpawnSpeed = 5;
                break;
            case 4:
                _maxWaves = 5;
                SpawnSpeed = 10;
                break;
            default:
                _maxWaves = 1;
                SpawnSpeed = 3;
                break;
        }
    }

    public void StartNewWave()
    {
        if (_currentWave <= _maxWaves)
        {
            UpdateEnemyWaveText();
            _timer = 0f;
        }
    }
    
    private void CheckWaveCompletion()
    {
        if (GameObject.FindObjectsOfType<Enemy>().Length == 0)
        {
            if (_currentWave >= _maxWaves)
            {
                Player.Instance.FinishGame();
                Debug.Log("All waves completed");
            }
            else
            {
                Player.Instance.TurnOffGame();
                _currentWave++;
                _enemiesInWave = 0;
                Debug.Log($"Wave: {_currentWave}/{_maxWaves} starting now");
            }
        }
    }

    private string GetDifficultyText()
    {
        switch (Player.Instance.Difficulty)
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
        Player.Instance.UpdateEnemyWaveText(_currentWave, _maxWaves, _maxEnemiesInWave[_currentWave - 1]);
    }
}
