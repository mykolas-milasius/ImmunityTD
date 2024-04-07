using NUnit.Framework;
using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.TestTools;

public class PlayerTests
{
    private Player player;
    private GameObject enemyGenerator;
    private TextMeshProUGUI coinsText;
    private TextMeshProUGUI scoreText;
    private TextMeshProUGUI killsText;
    private TextMeshProUGUI enemySpawnDelayText;

    // Setup for each test
    [SetUp]
    public void SetUp()
    {
        // Create a new Player object with necessary components
        GameObject playerGO = new GameObject("Player");
        player = playerGO.AddComponent<Player>();
        enemyGenerator = new GameObject("EnemyGenerator");
        player.enemyGenerator = enemyGenerator;

        // Set up UI Text components
        coinsText = new GameObject("CoinsText").AddComponent<TextMeshProUGUI>();
        scoreText = new GameObject("ScoreText").AddComponent<TextMeshProUGUI>();
        killsText = new GameObject("KillsText").AddComponent<TextMeshProUGUI>();
        enemySpawnDelayText = new GameObject("EnemySpawnDelayText").AddComponent<TextMeshProUGUI>();
        player.coinsText = coinsText;
        player.scoreText = scoreText;
        player.killsText = killsText;
        player.enemySpawnDelayText = enemySpawnDelayText;

        // Reset static fields to default for clean test start
        Player.coins = 100f;
        Player.score = 0;
        Player.kills = 0;
    }

    // Test initial state
    [Test]
    public void InitialState_IsCorrect()
    {
        Assert.AreEqual(100f, Player.coins);
        Assert.AreEqual(0, Player.score);
        Assert.AreEqual(0, Player.kills);
    }

    // Test adding coins
    [Test]
    public void AddCoins_IncreasesCoins()
    {
        Player.AddCoins(50f);
        Assert.AreEqual(150f, Player.coins);
    }

    // Test adding score
    [Test]
    public void AddScore_IncreasesScore()
    {
        Player.AddScore(10);
        Assert.AreEqual(10, Player.score);
    }

    // Test adding a kill
    [Test]
    public void AddKill_IncreasesKills()
    {
        Player.AddKill();
        Assert.AreEqual(1, Player.kills);
    }

    // Test adding multiple kills
    [Test]
    public void AddKills_IncreasesMultipleKills()
    {
        Player.AddKills(5);
        Assert.AreEqual(5, Player.kills);
    }

    // Test FixedUpdate updates UI elements
    [Test]
    public void FixedUpdate_UpdatesUIElements()
    {
        // Simulate FixedUpdate calls
        for (int i = 0; i < 5; i++)
        {
            player.FixedUpdate();
        }

        Assert.AreEqual("100", coinsText.text);
        Assert.AreEqual("0", scoreText.text);
        Assert.AreEqual("0", killsText.text);
    }

    [UnityTest]
    public IEnumerator FixedUpdate_ActivatesEnemyGeneratorAfterDelay()
    {
        // Simulate time until the generator should be activated
        float simulateTime = player.generatorDelay + 1f;
        for (float t = 0; t < simulateTime; t += Time.fixedDeltaTime)
        {
            // Wait for the next FixedUpdate cycle
            yield return new WaitForFixedUpdate();
        }

        // Now that sufficient time has passed, check the conditions
        Assert.IsTrue(enemyGenerator.activeSelf, "Enemy generator should be active after the delay.");
        Assert.IsFalse(enemySpawnDelayText.enabled, "Enemy spawn delay text should be disabled after the delay.");
    }

    // Cleanup after each test
    [TearDown]
    public void TearDown()
    {
        // Clean up objects and reset static fields to default
        GameObject.DestroyImmediate(player.gameObject);
        GameObject.DestroyImmediate(enemyGenerator);
        GameObject.DestroyImmediate(coinsText.gameObject);
        GameObject.DestroyImmediate(scoreText.gameObject);
        GameObject.DestroyImmediate(killsText.gameObject);
        GameObject.DestroyImmediate(enemySpawnDelayText.gameObject);

        Player.coins = 100f;
        Player.score = 0;
        Player.kills = 0;
    }
}
