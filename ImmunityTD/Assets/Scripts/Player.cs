using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance; // Used to access Player class from other scripts
    public static float Coins = 100f;
    public static int Score = 0;
    public static int Kills = 0;
    public float GeneratorDelay = 10f;
    public static int Health = 100;
    public int Difficulty = 2; // 1 - easy, 2 - medium, 3 - hard, 4 - slavery
    public bool DebugMode = false; // No delay for enemy generator, infinite coins
    
    private float _timer = 0f;
    private float _enemyWaveTextTimer = 5f;
    private bool _gameRunning = false;

    public GameObject EnemyGenerator;
    public TextMeshProUGUI CoinsText;
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI KillsText;
    public TextMeshProUGUI HealthText;
    public TextMeshProUGUI EnemySpawnDelayText;
    public TextMeshProUGUI EnemyWaveText;

    public void FixedUpdate()
    {
        if (!_gameRunning)
        {
            StartEnemyGenerator();
        }

        UpdateTaskbar();
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void AddCoins(float coinsToAdd)
    {
        Coins += coinsToAdd;
    }

    public static void AddScore(float scoreToAdd)
    {
        Score += (int)scoreToAdd;
    }

    public static void AddKill()
    {
        Kills++;
    }

    public static void AddKills(int killsToAdd)
    {
        Kills += killsToAdd;
    }
    public static void TakeDamage(int damage)
    {
        Health -= damage;
    }

    void StartEnemyGenerator()
    {
        if (!DebugMode)
        {
            if (_timer < GeneratorDelay)
            {
                _timer += Time.deltaTime;
                EnemySpawnDelayText.text = String.Format("Enemies spawn in: {0:f1} seconds", GeneratorDelay - _timer);
            }
            else {
                TurnOnGame();
            }
        }
        else
        {
            Coins = 10000000f;
            GeneratorDelay = 0.5f;
            TurnOnGame();
        }
    }

    void UpdateTaskbar()
    {
        NumberFormatInfo nfi = new NumberFormatInfo();
        nfi.NumberGroupSeparator = " ";
        nfi.NumberGroupSizes = new int[] { 3 };

        if (CoinsText != null)
        {
            CoinsText.text = Coins.ToString("n0", nfi);
        }

        if (ScoreText != null)
        {
            ScoreText.text = Score.ToString("n0", nfi);
        }

        if (KillsText != null)
        {
            KillsText.text = Kills.ToString("n0", nfi);
        }

        if (HealthText != null)
        {
            HealthText.text = Health.ToString("n0", nfi);
        }
    }

    public void TurnOnGame()
    {
        if (!_gameRunning)
        {
            _gameRunning = true;
            _timer = 0f;
            StartEnemyGenerator();
            EnemyGenerator.SetActive(true);
            EnemySpawnDelayText.gameObject.SetActive(false);
            Debug.Log("Game started");
        }
    }

    public void TurnOffGame()
    {
        if (_gameRunning)
        {
            _gameRunning = false;
            _timer = 0f;
            GeneratorDelay /= 2;
            EnemyGenerator.SetActive(false);
            EnemySpawnDelayText.gameObject.SetActive(true);
            Debug.Log("Game stopped");
        }
    }

    public void UpdateEnemyWaveText(int wave, int maxWaves, int enemiesInWave)
    {
        if (EnemyWaveText != null)
        {
            EnemyWaveText.text = $"Wave {wave}/{maxWaves}. Enemies in wave: {enemiesInWave}";
            EnemyWaveText.gameObject.SetActive(true);
            StartCoroutine(HideEnemyWaveTextAfterDelay(_enemyWaveTextTimer));
        }
    }

    public IEnumerator HideEnemyWaveTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (EnemyWaveText != null)
        {
            EnemyWaveText.gameObject.SetActive(false);
        }
    }
}
