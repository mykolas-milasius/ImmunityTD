using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static float coins = 100f;
    public static int score = 0;
    public static int kills = 0;
    private float timer = 0f;
    public float generatorDelay = 10f;
    public static int health = 100;

    public TextMeshProUGUI coinsText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI killsText;
    public TextMeshProUGUI healthText;
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
}
