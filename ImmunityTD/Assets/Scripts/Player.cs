using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    /// <summary>
    /// Current number of coins
    /// </summary>
    public static float coins = 100f;

    /// <summary>
    /// Current number of coins
    /// </summary>
    public static int score = 0;

    /// <summary>
    /// Current number of kills
    /// </summary>
    public static int kills = 0;

    /// <summary>
    /// Timer for enemy generator
    /// </summary>
    private float timer = 0f;

    /// <summary>
    /// Delay for enemy generator
    /// </summary>
    public float generatorDelay = 10f;

    /// <summary>
    /// Debug mode - infinite coins, no delay for enemy generator
    /// </summary>
    public bool debugMode = false;

    public TextMeshProUGUI coinsText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI killsText;
    public GameObject enemyGenerator;
    public TextMeshProUGUI enemySpawnDelayText;
    
    public void FixedUpdate()
    {
        if (coinsText != null)
        {
            coinsText.text = coins.ToString();
        }

        if (scoreText != null)
        {
            scoreText.text = score.ToString();
        }

        if (killsText != null)
        {
            killsText.text = kills.ToString();
        }

        if (debugMode)
        {
            InitializeDebug();
        }
        else
        {
            Initialize();
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

    void Initialize()
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

    void InitializeDebug()
    {
        coins = 1000000f;
        generatorDelay = 0.1f;
        enemyGenerator.SetActive(true);
        enemySpawnDelayText.enabled = false;
    }
}
