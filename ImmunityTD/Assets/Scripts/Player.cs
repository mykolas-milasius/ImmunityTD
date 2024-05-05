using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    /// <summary>
    /// Player instance to access from other scripts
    /// </summary>
    public static Player Instance;
    public static float coins = 100f;
    public static int score = 0;
    public static int kills = 0;
    private float timer = 0f;
    public float generatorDelay = 10f;
    public static int health = 100;
    public float enemyWaveTextTimer = 5f;

    /// <summary>
    /// Debug mode - infinite coins, no delay for enemy generator
    /// </summary>
    public bool debugMode = false;
    private bool gameRunning = false;

    public TextMeshProUGUI coinsText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI killsText;
    public TextMeshProUGUI healthText;
    public GameObject enemyGenerator;
    public TextMeshProUGUI enemySpawnDelayText;
    public TextMeshProUGUI enemyWaveText;
    
    public void FixedUpdate()
    {
        UpdateTaskbar();
        if (!gameRunning)
        {
            enemySpawnDelayText.gameObject.SetActive(true);
            StartEnemyGenerator();
        }
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
        if(healthText != null)
        {
            healthText.text = health.ToString();
        }

        if (timer < generatorDelay)
        {
            timer += Time.deltaTime;
            if (enemySpawnDelayText != null)
            {
                enemySpawnDelayText.text = String.Format("Enemies spawn in: {0,3} seconds", Math.Round(generatorDelay - timer, 1).ToString());
            }
        }
        else
        {
            if (enemyGenerator != null)
            {
                enemyGenerator.SetActive(true);
            }
            if (enemySpawnDelayText != null)
            {
                enemySpawnDelayText.enabled = false;
            }
        }
    }

    public static void AddCoins(float coinsToAdd)
    {
        coins += coinsToAdd;
    }

    public static void AddScore(float scoreToAdd)
    {
        score += (int)scoreToAdd;
    }

    public static void AddKill()
    {
        kills++;
    }

    public static void AddKills(int killsToAdd)
    {
        kills += killsToAdd;
    }
    public static void TakeDamage(int damage)
    {
        health -= damage;
    }

    void StartEnemyGenerator()
    {
        if (!debugMode)
        {
            if (timer < generatorDelay)
            {
                timer += Time.deltaTime;
                if (enemySpawnDelayText != null)
                {
                    enemySpawnDelayText.text = String.Format("Enemies spawn in: {0:f1} seconds", generatorDelay - timer);
                }
            }
            else {
                if (enemyGenerator != null && enemySpawnDelayText != null)
                {
                    TurnOnGame();
                }
            }
        }
        else
        {
            coins = 10000000f;
            generatorDelay = 1f;
            TurnOnGame();
        }
    }

    void UpdateTaskbar()
    {
        NumberFormatInfo nfi = new NumberFormatInfo();
        nfi.NumberGroupSeparator = " ";
        nfi.NumberGroupSizes = new int[] { 3 };

        if (coinsText != null)
        {
            coinsText.text = coins.ToString("n0", nfi);
        }

        if (scoreText != null)
        {
            scoreText.text = score.ToString("n0", nfi);
        }

        if (killsText != null)
        {
            killsText.text = kills.ToString("n0", nfi);
        }
    }

    public void TurnOnGame()
    {
        gameRunning = true;
        enemyGenerator.SetActive(true);
        enemySpawnDelayText.gameObject.SetActive(false);
        Debug.Log("Game started");
    }

    public void TurnOffGame()
    {
        gameRunning = false;
        timer = 0f;
        generatorDelay /= 2;
        enemyGenerator.SetActive(false);
        enemySpawnDelayText.gameObject.SetActive(true);
        Debug.Log("Game stopped");
    }

    public void UpdateEnemyWaveText(int wave, int maxWaves, int enemiesInWave)
    {
        if (enemyWaveText != null)
        {
            enemyWaveText.text = $"Wave {wave}/{maxWaves}. Enemies in wave: {enemiesInWave}";
            enemyWaveText.gameObject.SetActive(true);
            StartCoroutine(HideEnemyWaveTextAfterDelay(enemyWaveTextTimer));
        }
    }

    private IEnumerator HideEnemyWaveTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (enemyWaveText != null)
        {
            enemyWaveText.gameObject.SetActive(false);
        }
    }
}
