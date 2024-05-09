using System;
using System.Collections;
using System.Reflection;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;
using Assets.Scripts;

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
        tower.BulletPrefab = bulletPrefab;

        // Mocking the range preview GameObject
        tower.RangePreview = new GameObject("RangePreview");

        // Enemy setup
        enemyGameObject = new GameObject("Enemy");
        enemy = enemyGameObject.AddComponent<Enemy>();
        enemy.MaxHealth = 100;  // Example health

        // Assign mock UI components
        tower.DamageText = new GameObject().AddComponent<TextMeshProUGUI>();
        tower.AttackSpeedText = new GameObject().AddComponent<TextMeshProUGUI>();
        tower.RangeText = new GameObject().AddComponent<TextMeshProUGUI>();
        tower.UpgradeDamageText = new GameObject().AddComponent<TextMeshProUGUI>();
        tower.UpgradeAttackSpeedText = new GameObject().AddComponent<TextMeshProUGUI>();
        tower.UpgradeRangeText = new GameObject().AddComponent<TextMeshProUGUI>();
        tower.UpgradePriceText = new GameObject().AddComponent<TextMeshProUGUI>();

        // Mocking the Button component
        GameObject buttonGameObject = new GameObject();
        tower.Button = buttonGameObject.AddComponent<Button>();

        var healthBarGO = new GameObject("HealthBarForeground");
        enemy.HealthBarForeground = healthBarGO.AddComponent<RectTransform>();

        // Initialize Tower with valid starting values
        tower.Damage = 50f;
        tower.AttackSpeed = 1f;
        tower.Range = 50f;
        tower.StartPrice = 50f;

        // Initialize UI components for Enemy
        GameObject damageTextGO = new GameObject("DamageText");
        enemy.DamageText = damageTextGO.AddComponent<TextMeshProUGUI>();
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

        Assert.IsTrue(tower.Entered, "Tower should mark enemy as entered when in range.");
        Assert.IsFalse(tower.Exited, "Tower should not mark enemy as exited immediately after entry.");
    }

    [Test]
    public void Tower_DisengagesEnemy_WhenOutOfRange()
    {
        tower.EnemyEnteredRange(enemyGameObject);  // Enter range first
        tower.EnemyExitedRange(enemyGameObject);  // Then exit

        Assert.IsTrue(tower.Exited, "Tower should mark enemy as exited when out of range.");
    }

    [Test]
    public void Tower_UpgradesCorrectly()
    {
        Player.Coins = 1000f;

        float initialDamage = tower.Damage;
        float initialAttackSpeed = tower.AttackSpeed;
        float initialRange = tower.Range;

        tower.Upgrade();

        Assert.Greater(tower.Damage, initialDamage, "Damage should increase after an upgrade.");
        Assert.Greater(tower.AttackSpeed, initialAttackSpeed, "Attack Speed should increase after an upgrade.");
        Assert.Greater(tower.Range, initialRange, "Range should increase after an upgrade.");
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

        Assert.AreEqual(tower.Damage.ToString(), tower.DamageText.text, "Damage text should match Tower's Damage.");
        Assert.AreEqual(tower.AttackSpeed.ToString(), tower.AttackSpeedText.text, "Attack Speed text should match Tower's attack Speed.");
        Assert.AreEqual(tower.Range.ToString(), tower.RangeText.text, "Range text should match Tower's range.");
    }

    [Test]
    public void Tower_AttacksRespectCooldown()
    {
        tower.EnemyEnteredRange(enemyGameObject);
        tower.Update();  // First attack

        Bullet firstBullet = GameObject.FindObjectOfType<Bullet>();

        // Simulate just under cooldown time passing
        float justUnderCooldownTime = 1f / tower.AttackSpeed - 0.1f;
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
        tower.Range = 100f; // Set new range
        tower.Start(); // To trigger range preview scaling

        float expectedDiameter = (tower.Range / 100) + 1;
        Vector3 expectedScale = new Vector3(expectedDiameter, expectedDiameter, 1);

        Assert.AreEqual(expectedScale, tower.RangePreview.transform.localScale, "Range preview scale should match Tower range.");
    }

    [Test]
    public void UI_TextFields_Update_WithTowerAttributes()
    {

        tower.Update();

        Assert.AreEqual(tower.Damage.ToString(), tower.DamageText.text, "Damage text should match Tower's Damage.");
        Assert.AreEqual(tower.AttackSpeed.ToString(), tower.AttackSpeedText.text, "Attack Speed text should match Tower's attack Speed.");
        Assert.AreEqual(tower.Range.ToString(), tower.RangeText.text, "Range text should match Tower's range.");
    }

    [Test]
    public void UI_Warns_WhenTextFieldsNotAssigned()
    {
        GameObject.DestroyImmediate(tower.DamageText.gameObject);
        tower.DamageText = null; // Simulate unassigned text component

        LogAssert.Expect(LogType.Warning, "UI text fields not assigned in Tower script.");
        tower.Update(); // Trigger the Update method to check for warnings
    }
    
    // Saras turi sutvarkyti shop ir Tower meniu
    // [Test]
    // public void Button_Interactability_IsInitiallyFalse()
    // {
    //     Assert.IsFalse(Tower.Button.interactable, "Upgrade Button should initially be not interactable.");
    // }

    [Test]
    public void Bullet_IsInstantiatedAndTargetsFirstEnemy_InRange()
    {
        // Simulate an enemy entering range
        tower.EnemyEnteredRange(enemyGameObject);
        tower.Update(); // Force the Tower to shoot

        Bullet bullet = GameObject.FindObjectOfType<Bullet>();
        Assert.IsNotNull(bullet, "A bullet should be instantiated when the Tower shoots.");

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
        GameObject.DestroyImmediate(tower.BulletPrefab.GetComponent<Bullet>());

        LogAssert.Expect(LogType.Warning, "Bullet prefab does not contain Bullet component.");
        tower.Shoot(); // Trigger the shoot method that should log the warning
    }

    [Test]
    public void Tower_DealsDamageToEnemy()
    {
        enemy.Start();
        enemy.MaxHealth = 100f;
        bool wasAliveBefore = enemy.IsAlive();
        tower.Damage = 100f;

        tower.DealDamage(enemyGameObject); // Apply Damage

        bool isAliveAfter = enemy.IsAlive();
        Assert.IsTrue(wasAliveBefore, "Enemy should be alive before taking Damage.");
        Assert.IsFalse(isAliveAfter, "Enemy should not be alive after taking enough Damage.");
    }

}
