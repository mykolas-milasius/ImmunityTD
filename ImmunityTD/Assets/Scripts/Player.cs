using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static float coins = 100f;
    public static int score = 0;
    public static int kills = 0;
    public static TextMeshProUGUI coinsText;
    public static TextMeshProUGUI scoreText;
    public static TextMeshProUGUI killsText;

    public void FixedUpdate()
    {
        UpdateTaskbar();
    }

    public static void UpdateTaskbar() {
        coinsText.text = coins.ToString();
        scoreText.text = score.ToString();
        killsText.text = kills.ToString();
    }

    public static void AddCoins(float coinsToAdd){
        coins += coinsToAdd;
    }

    public static void AddScore(float scoreToAdd){
        score += (int)scoreToAdd;
    }

    public static void AddKill(){
        kills++;
    }

    public static void AddKills(int killsToAdd){
        kills += killsToAdd;
    }
}
