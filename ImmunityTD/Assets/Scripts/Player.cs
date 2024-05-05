using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance; // Used to access Player class from other scripts
    public static float coins = 100f;
    public static int score = 0;
    public static int kills = 0;
    public float generatorDelay = 10f;
    public static int health = 100;
    public float enemyWaveTextTimer = 5f;
    public bool debugMode = false; // No delay for enemy generator, infinite coins
    
    private float timer = 0f;
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
        if (!gameRunning)
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
                enemySpawnDelayText.text = String.Format("Enemies spawn in: {0:f1} seconds", generatorDelay - timer);
            }
            else {
                TurnOnGame();
            }
        }
        else
        {
            coins = 10000000f;
            generatorDelay = 0.5f;
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

        if (healthText != null)
        {
            healthText.text = health.ToString("n0", nfi);
        }
    }

    public void TurnOnGame()
    {
        if (!gameRunning)
        {
            gameRunning = true;
            StartEnemyGenerator();
            enemyGenerator.SetActive(true);
            enemySpawnDelayText.gameObject.SetActive(false);
            Debug.Log("Game started");
        }
    }

    public void TurnOffGame()
    {
        if (gameRunning)
        {
            gameRunning = false;
            timer = 0f;
            generatorDelay /= 2;
            enemyGenerator.SetActive(false);
            enemySpawnDelayText.gameObject.SetActive(true);
            Debug.Log("Game stopped");
        }
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
