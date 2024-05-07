using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Reflection;
using TMPro;

public class EnemyTests
{
    private GameObject enemyGameObject;
    private Enemy enemy;
    private float initialHealthBarWidth;

    // Setup for each test
    [SetUp]
    public void Setup()
    {
        enemyGameObject = new GameObject();
        enemy = enemyGameObject.AddComponent<Enemy>();
        enemy.HealthBarForeground = new GameObject().AddComponent<RectTransform>();
        enemy.DamageText = new GameObject().AddComponent<TextMeshProUGUI>();

        // Mock SpriteRenderer to test DimSprite indirectly
        enemy.gameObject.AddComponent<SpriteRenderer>();

        // Trigger the Start method manually
        enemy.SendMessage("Start");

        initialHealthBarWidth = enemy.HealthBarForeground.sizeDelta.x;
    }

    // Test to ensure the enemy is initialized correctly
    [Test]
    public void Enemy_InitializedCorrectly()
    {
        var currentHealthField = typeof(Enemy).GetField("currentHealth", BindingFlags.NonPublic | BindingFlags.Instance);
        float currentHealth = (float)currentHealthField.GetValue(enemy);

        Assert.AreEqual(enemy.MaxHealth, currentHealth, "Current health should be initialized to max health");
        Assert.AreNotEqual(0, initialHealthBarWidth, "Health bar should have a non-zero width");
    }

    // Test to ensure the enemy takes Damage correctly and updates its health
    [Test]
    public void Enemy_TakesDamageCorrectly()
    {
        float damage = 20f;
        enemy.TakeDamage(damage);

        var currentHealthField = typeof(Enemy).GetField("currentHealth", BindingFlags.NonPublic | BindingFlags.Instance);
        float currentHealth = (float)currentHealthField.GetValue(enemy);

        Assert.AreEqual(enemy.MaxHealth - damage, currentHealth, "Health should decrease by the Damage amount");

        float expectedWidth = (currentHealth / enemy.MaxHealth) * initialHealthBarWidth;
        Assert.AreEqual(expectedWidth, enemy.HealthBarForeground.sizeDelta.x, "Health bar width should decrease proportionally to health loss");
    }

    // Test to ensure the enemy dies correctly when health reaches zero
    [Test]
    public void Enemy_DiesCorrectly()
    {
        enemy.TakeDamage(enemy.MaxHealth);

        var currentHealthField = typeof(Enemy).GetField("currentHealth", BindingFlags.NonPublic | BindingFlags.Instance);
        float currentHealth = (float)currentHealthField.GetValue(enemy);

        Assert.AreEqual(0, currentHealth, "Health should be zero when enemy dies");
    }

    // Test to check if the sprite dims when taking Damage
    [Test]
    public void Enemy_SpriteDimsOnDamage()
    {
        enemy.TakeDamage(10f);
        var spriteRenderer = enemy.GetComponent<SpriteRenderer>();

        Assert.AreEqual(0.8f, spriteRenderer.color.r, "Sprite's red channel should dim to 0.8 when taking Damage");
    }

    // Test to ensure the Damage text displays and hides correctly
    [Test]
    public void Enemy_DamageTextDisplaysAndHides()
    {
        enemy.TakeDamage(10f);

        Assert.IsTrue(enemy.DamageText.enabled, "Damage text should be enabled after taking Damage");
        Assert.AreEqual("-10", enemy.DamageText.text, "Damage text should display the correct Damage value");

        // Assuming you have a mechanism to call `DisableDamageText` after a delay, you can simulate this call here
        enemy.SendMessage("DisableDamageText");
        Assert.IsFalse(enemy.DamageText.enabled, "Damage text should be disabled after a delay");
    }

    // Test to ensure excessive Damage sets health to zero and doesn't go negative
    [Test]
    public void Enemy_HealthDoesNotGoNegative()
    {
        enemy.TakeDamage(enemy.MaxHealth * 2);  // Deal double the max health as Damage

        var currentHealthField = typeof(Enemy).GetField("currentHealth", BindingFlags.NonPublic | BindingFlags.Instance);
        float currentHealth = (float)currentHealthField.GetValue(enemy);

        Assert.AreEqual(0, currentHealth, "Health should not go below zero");
        Assert.AreEqual(0, enemy.HealthBarForeground.sizeDelta.x, "Health bar width should be zero when health is zero");
    }

    // Test to ensure the health bar does not increase in size beyond its original width
    [Test]
    public void Enemy_HealthBarDoesNotExceedOriginalWidth()
    {
        enemy.TakeDamage(-20f);  // Apply negative Damage, theoretically healing the enemy

        Assert.AreEqual(initialHealthBarWidth, enemy.HealthBarForeground.sizeDelta.x, "Health bar width should not exceed its original width");
    }

    // Test to check if the sprite dims and resets correctly on multiple Damage instances
    [Test]
    public void Enemy_SpriteDimsAndResetsCorrectly()
    {
        enemy.TakeDamage(10f);
        var spriteRenderer = enemy.GetComponent<SpriteRenderer>();
        Color dimmedColor = spriteRenderer.color;

        enemy.SendMessage("ResetSpriteColor");  // Reset color manually for testing
        Color resetColor = spriteRenderer.color;

        Assert.AreEqual(0.8f, dimmedColor.r, "Sprite's red channel should dim to 0.8 when taking Damage");
        Assert.AreEqual(1f, resetColor.r, "Sprite's red channel should reset to 1 after a short duration");
    }

    // Test to check if the Damage text displays correctly for multiple Damage instances
    [Test]
    public void Enemy_DamageTextUpdatesCorrectlyForMultipleDamages()
    {
        enemy.TakeDamage(10f);
        Assert.AreEqual("-10", enemy.DamageText.text, "Damage text should display correct Damage value for the first Damage instance");

        enemy.TakeDamage(5f);
        Assert.AreEqual("-5", enemy.DamageText.text, "Damage text should update to display correct Damage value for the second Damage instance");
    }

    // Test to ensure player stats are updated upon enemy death
    [Test]
    public void Player_StatsUpdatedOnEnemyDeath()
    {
        GameObject playerGameObject = new GameObject();
        Player player = playerGameObject.AddComponent<Player>();

        // Record initial player stats
        float initialCoins = Player.Coins;
        int initialKills = Player.Kills;
        int initialScore = Player.Score;

        // Kill the enemy
        enemy.TakeDamage(enemy.MaxHealth);

        // Since enemy death updates are static in the Player class, we don't need to access player instance
        Assert.AreEqual(initialCoins + enemy.CoinsWhenDied, Player.Coins, "Player coins should increase by the coinsWhenDied value of the enemy");
        Assert.AreEqual(initialKills + 1, Player.Kills, "Player kill count should increase by 1");
        Assert.AreEqual(initialScore + enemy.CoinsWhenDied, Player.Score, "Player score should increase by the coinsWhenDied value of the enemy");
    }

    // Cleanup after each test
    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(enemyGameObject);
    }
}
