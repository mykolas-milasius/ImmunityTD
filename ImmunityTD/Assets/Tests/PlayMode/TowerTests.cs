using System;
using System.Collections;
using System.Reflection;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

public class TowerTests
{
    private GameObject towerGameObject;
    private Tower tower;
    private GameObject enemyGameObject;
    private Enemy enemy;

    [SetUp]
    public void SetUp()
    {
        // Tower setup
        towerGameObject = new GameObject("Tower");
        tower = towerGameObject.AddComponent<Tower>();

        // Mocking the bullet prefab with a simple GameObject
        GameObject bulletPrefab = new GameObject("BulletPrefab");
        bulletPrefab.AddComponent<Bullet>();  // Assuming Bullet has a no-arg constructor
        tower.bulletPrefab = bulletPrefab;

        // Mocking the range preview GameObject
        tower.rangePreview = new GameObject("RangePreview");

        // Enemy setup
        enemyGameObject = new GameObject("Enemy");
        enemy = enemyGameObject.AddComponent<Enemy>();
        enemy.maxHealth = 100;  // Example health

        // Assign mock UI components
        tower.damageText = new GameObject().AddComponent<TextMeshProUGUI>();
        tower.attackSpeedText = new GameObject().AddComponent<TextMeshProUGUI>();
        tower.rangeText = new GameObject().AddComponent<TextMeshProUGUI>();
        tower.upgradeDamageText = new GameObject().AddComponent<TextMeshProUGUI>();
        tower.upgradeAttackSpeedText = new GameObject().AddComponent<TextMeshProUGUI>();
        tower.upgradeRangeText = new GameObject().AddComponent<TextMeshProUGUI>();
        tower.upgradePriceText = new GameObject().AddComponent<TextMeshProUGUI>();

        // Mocking the Button component
        GameObject buttonGameObject = new GameObject();
        tower.button = buttonGameObject.AddComponent<Button>();

        var healthBarGO = new GameObject("HealthBarForeground");
        enemy.healthBarForeground = healthBarGO.AddComponent<RectTransform>();

        // Initialize Tower with valid starting values
        tower.damage = 50f;
        tower.attackSpeed = 1f;
        tower.range = 50f;
        tower.startPrice = 50f;

        // Initialize UI components for Enemy
        GameObject damageTextGO = new GameObject("DamageText");
        enemy.damageText = damageTextGO.AddComponent<TextMeshProUGUI>();
    }

    [TearDown]
    public void TearDown()
    {
        GameObject.DestroyImmediate(towerGameObject);
        GameObject.DestroyImmediate(enemyGameObject);
    }

    [Test]
    public void Tower_EngagesEnemy_WhenInRange()
    {
        tower.EnemyEnteredRange(enemyGameObject);

        Assert.IsTrue(tower.entered, "Tower should mark enemy as entered when in range.");
        Assert.IsFalse(tower.exited, "Tower should not mark enemy as exited immediately after entry.");
    }

    [Test]
    public void Tower_DisengagesEnemy_WhenOutOfRange()
    {
        tower.EnemyEnteredRange(enemyGameObject);  // Enter range first
        tower.EnemyExitedRange(enemyGameObject);  // Then exit

        Assert.IsTrue(tower.exited, "Tower should mark enemy as exited when out of range.");
    }

    [Test]
    public void Tower_UpgradesCorrectly()
    {
        Player.Coins = 1000f;

        float initialDamage = tower.damage;
        float initialAttackSpeed = tower.attackSpeed;
        float initialRange = tower.range;

        tower.Upgrade();

        Assert.Greater(tower.damage, initialDamage, "Damage should increase after an upgrade.");
        Assert.Greater(tower.attackSpeed, initialAttackSpeed, "Attack speed should increase after an upgrade.");
        Assert.Greater(tower.range, initialRange, "Range should increase after an upgrade.");
    }

    [Test]
    public void Tower_ShootsAtEnemy_WhenInRange()
    {
        tower.EnemyEnteredRange(enemyGameObject);
        tower.Update();

        Bullet bullet = GameObject.FindObjectOfType<Bullet>();
        Assert.IsNotNull(bullet, "Tower should shoot a bullet when an enemy is in range.");
    }

    [Test]
    public void Tower_UIUpdates_WithCorrectValues()
    {
        tower.Update();

        Assert.AreEqual(tower.damage.ToString(), tower.damageText.text, "Damage text should match tower's damage.");
        Assert.AreEqual(tower.attackSpeed.ToString(), tower.attackSpeedText.text, "Attack speed text should match tower's attack speed.");
        Assert.AreEqual(tower.range.ToString(), tower.rangeText.text, "Range text should match tower's range.");
    }

    [Test]
    public void Tower_AttacksRespectCooldown()
    {
        tower.EnemyEnteredRange(enemyGameObject);
        tower.Update();  // First attack

        Bullet firstBullet = GameObject.FindObjectOfType<Bullet>();

        // Simulate just under cooldown time passing
        float justUnderCooldownTime = 1f / tower.attackSpeed - 0.1f;
        for (float time = 0; time < justUnderCooldownTime; time += Time.deltaTime)
        {
            tower.Update();
        }

        Bullet secondBullet = GameObject.FindObjectOfType<Bullet>();

        // Assert that no new bullet was created in this time
        Assert.AreEqual(firstBullet, secondBullet, "Tower should not attack again before cooldown period is over.");
    }

    [Test]
    public void RangePreview_ScalesCorrectly_WithTowerRange()
    {
        tower.range = 100f; // Set new range
        tower.Start(); // To trigger range preview scaling

        float expectedDiameter = (tower.range / 100) + 1;
        Vector3 expectedScale = new Vector3(expectedDiameter, expectedDiameter, 1);

        Assert.AreEqual(expectedScale, tower.rangePreview.transform.localScale, "Range preview scale should match tower range.");
    }

    [Test]
    public void UI_TextFields_Update_WithTowerAttributes()
    {

        tower.Update();

        Assert.AreEqual(tower.damage.ToString(), tower.damageText.text, "Damage text should match tower's damage.");
        Assert.AreEqual(tower.attackSpeed.ToString(), tower.attackSpeedText.text, "Attack speed text should match tower's attack speed.");
        Assert.AreEqual(tower.range.ToString(), tower.rangeText.text, "Range text should match tower's range.");
    }

    [Test]
    public void UI_Warns_WhenTextFieldsNotAssigned()
    {
        GameObject.DestroyImmediate(tower.damageText.gameObject);
        tower.damageText = null; // Simulate unassigned text component

        LogAssert.Expect(LogType.Warning, "UI text fields not assigned in Tower script.");
        tower.Update(); // Trigger the Update method to check for warnings
    }
    
    // Saras turi sutvarkyti shop ir tower meniu
    // [Test]
    // public void Button_Interactability_IsInitiallyFalse()
    // {
    //     Assert.IsFalse(tower.button.interactable, "Upgrade button should initially be not interactable.");
    // }

    [Test]
    public void Bullet_IsInstantiatedAndTargetsFirstEnemy_InRange()
    {
        // Simulate an enemy entering range
        tower.EnemyEnteredRange(enemyGameObject);
        tower.Update(); // Force the tower to shoot

        Bullet bullet = GameObject.FindObjectOfType<Bullet>();
        Assert.IsNotNull(bullet, "A bullet should be instantiated when the tower shoots.");

        // Using reflection to get the value of the 'target' field
        Type bulletType = bullet.GetType();
        FieldInfo targetField = bulletType.GetField("target", BindingFlags.NonPublic | BindingFlags.Instance);
        GameObject target = (GameObject)targetField.GetValue(bullet);

        // Ensure the bullet's target is the first enemy in range
        Assert.AreEqual(enemyGameObject, target, "Bullet should target the first enemy in range.");
    }

    [Test]
    public void WarningLogged_WhenBulletPrefabMissingBulletComponent()
    {
        // Remove the Bullet component to simulate a misconfigured prefab
        GameObject.DestroyImmediate(tower.bulletPrefab.GetComponent<Bullet>());

        LogAssert.Expect(LogType.Warning, "Bullet prefab does not contain Bullet component.");
        tower.Shoot(); // Trigger the shoot method that should log the warning
    }

    [Test]
    public void Tower_DealsDamageToEnemy()
    {
        enemy.Start();
        enemy.maxHealth = 100f;
        bool wasAliveBefore = enemy.IsAlive();
        tower.damage = 100f;

        tower.DealDamage(enemyGameObject); // Apply damage

        bool isAliveAfter = enemy.IsAlive();
        Assert.IsTrue(wasAliveBefore, "Enemy should be alive before taking damage.");
        Assert.IsFalse(isAliveAfter, "Enemy should not be alive after taking enough damage.");
    }

}
