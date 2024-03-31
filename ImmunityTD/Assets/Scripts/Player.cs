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
    private float generatorDelay = 10f;

    public TextMeshProUGUI coinsText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI killsText;
    public GameObject enemyGenerator;
    public TextMeshProUGUI enemySpawnDelayText;
    
    public void FixedUpdate()
    {
        coinsText.text = coins.ToString();
        scoreText.text = score.ToString();
        killsText.text = kills.ToString();
        // pakeisti kad tikrintu tik pradzioj ir nevalgytu resursu
        if (timer < generatorDelay)
        {
            timer += Time.deltaTime;
            enemySpawnDelayText.text = String.Format("Enemies spawn in: {0:3} seconds", Math.Round(generatorDelay - timer, 1).ToString());
        }
        else {
            enemyGenerator.SetActive(true);
            enemySpawnDelayText.enabled = false;
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
}
