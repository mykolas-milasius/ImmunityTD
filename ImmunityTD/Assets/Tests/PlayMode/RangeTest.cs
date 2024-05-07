using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

public class RangeTest
{
    private GameObject towerObject;
    private Tower tower;
    private GameObject preview;
    private GameObject bulletPrefab;
    private TextMeshProUGUI damageText;
    private TextMeshProUGUI attackSpeedText;
    private TextMeshProUGUI rangeText;
    private GameObject rangePreview;
    private TextMeshProUGUI upgradeDamageText;
    private TextMeshProUGUI upgradeAttackSpeedText;
    private TextMeshProUGUI upgradeRangeText;
    private TextMeshProUGUI upgradePriceText;
    private Button button;


    [SetUp]
    public void Setup()
    {
        towerObject = new GameObject();
        tower = towerObject.AddComponent<Tower>();
        preview = new GameObject();
        tower.RangePreview = preview;
        bulletPrefab = new GameObject();
        tower.BulletPrefab = bulletPrefab;
        GameObject text = new GameObject();
        GameObject text1 = new GameObject();
        GameObject text2 = new GameObject();
        GameObject text3 = new GameObject();
        GameObject text4 = new GameObject();
        GameObject text5 = new GameObject();
        GameObject text6 = new GameObject();
        GameObject but = new GameObject();
        damageText = text.AddComponent<TextMeshProUGUI>();
        attackSpeedText = text1.AddComponent<TextMeshProUGUI>();
        rangeText = text2.AddComponent<TextMeshProUGUI>();
        rangePreview = new GameObject();
        upgradeDamageText = text3.AddComponent<TextMeshProUGUI>();
        upgradeAttackSpeedText = text4.AddComponent<TextMeshProUGUI>();
        upgradeRangeText = text5.AddComponent<TextMeshProUGUI>();
        upgradePriceText = text6.AddComponent<TextMeshProUGUI>();
        button = but.AddComponent<Button>();
        tower.DamageText = damageText;
        tower.AttackSpeedText = attackSpeedText;
        tower.RangeText = rangeText;
        tower.RangePreview = rangePreview;
        tower.UpgradeDamageText = upgradeDamageText;
        tower.UpgradeAttackSpeedText = upgradeAttackSpeedText;
        tower.UpgradeRangeText = upgradeRangeText;
        tower.UpgradePriceText = upgradePriceText;
        tower.Button = button;
    }

    [TearDown]
    public void Teardown()
    {
        GameObject.DestroyImmediate(towerObject);
        GameObject.DestroyImmediate(tower);
        GameObject.DestroyImmediate(preview);
        GameObject.DestroyImmediate(bulletPrefab);
        GameObject.DestroyImmediate(damageText);
        GameObject.DestroyImmediate(attackSpeedText);
        GameObject.DestroyImmediate(rangeText);
        GameObject.DestroyImmediate(upgradePriceText);
        GameObject.DestroyImmediate(upgradeDamageText);
        GameObject.DestroyImmediate(upgradeAttackSpeedText);
        GameObject.DestroyImmediate(upgradeRangeText);
        GameObject.DestroyImmediate(button);
    }

    [UnityTest]
    public IEnumerator OnTriggerEnter2D_EnemyInRange_EnemyEnteredRangeCalled()
    {
        // Arrange
        GameObject rangeObject = new GameObject();
        TowerRange towerRange = rangeObject.AddComponent<TowerRange>();
        towerRange.ParentTower = tower;

        GameObject enemyObject = new GameObject();
        enemyObject.tag = "Enemy";
        Collider2D enemyCollider = enemyObject.AddComponent<BoxCollider2D>();
        enemyCollider.isTrigger = true;

        // Act
        towerRange.OnTriggerEnter2D(enemyCollider);

        // Assert
        yield return null; // Wait for a frame to process Unity's OnTrigger logic
        Assert.IsTrue(tower.Entered); // Ensure EnemyEnteredRange method was called
    }

    [UnityTest]
    public IEnumerator OnTriggerExit2D_EnemyOutOfRange_EnemyExitedRangeCalled()
    {
        GameObject rangeObject = new GameObject();
        TowerRange towerRange = rangeObject.AddComponent<TowerRange>();
        towerRange.ParentTower = tower;

        GameObject enemyObject = new GameObject();
        enemyObject.tag = "Enemy";
        Collider2D enemyCollider = enemyObject.AddComponent<BoxCollider2D>();
        enemyCollider.isTrigger = true;

        towerRange.OnTriggerEnter2D(enemyCollider); // Enemy enters range first

        // Act
        towerRange.OnTriggerExit2D(enemyCollider);

        // Assert
        yield return null; // Wait for a frame to process Unity's OnTrigger logic
        Assert.IsTrue(tower.Exited); // Ensure EnemyExitedRange method was called
    }
}
