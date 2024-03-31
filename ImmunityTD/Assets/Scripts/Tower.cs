using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tower : MonoBehaviour
{
    public float damage;
    public float attackSpeed; // attacks per second
    public float range;
    public float startPrice;

    public TextMeshProUGUI damageText;
    public TextMeshProUGUI attackSpeedText;
    public TextMeshProUGUI rangeText;
    public GameObject rangePreview;

    public TextMeshProUGUI upgradeDamageText;
    public TextMeshProUGUI upgradeAttackSpeedText;
    public TextMeshProUGUI upgradeRangeText;
    public TextMeshProUGUI upgradePriceText;

    public Button button;

    private List<GameObject> enemiesInRange = new List<GameObject>();
    private float attackCooldown;
    private float upgradeDamage;
    private float upgradeAttackSpeed;
    private float upgradeRange;
    private float upgradePrice;

    public void Start()
    {
        upgradePrice = (float)Math.Round(startPrice * 2, 1);
        upgradeDamage = (float)Math.Round(damage * 1.2, 1);
        upgradeAttackSpeed = (float)Math.Round(attackSpeed * 1.2, 1);
        upgradeRange = (float)Math.Round(range * 1.2, 1);
        button.interactable = false;

        UpdateRange();
        attackCooldown = 1f / attackSpeed;
    }

    public void Update(){

        damageText.text = damage.ToString();
        attackSpeedText.text = attackSpeed.ToString();
        rangeText.text = range.ToString();

        upgradeDamageText.text = upgradeDamage.ToString();
        upgradeAttackSpeedText.text = upgradeAttackSpeed.ToString();
        upgradeRangeText.text = upgradeRange.ToString();

        upgradePriceText.text = upgradePrice.ToString();

        attackCooldown -= Time.deltaTime;

        if (attackCooldown <= 0f && enemiesInRange.Count > 0)
        {
            attackCooldown = 1f / attackSpeed;
            DealDamage(enemiesInRange[0]);
        }
        if(Player.coins >= upgradePrice)
        {
            button.interactable = true;
        }
        else
        {
            button.interactable = false;
        }
    }
    public void Upgrade()
    {  
        damage = (float)(damage * 1.2);
        attackSpeed = (float)(attackSpeed * 1.2);
        range = (float)(range * 1.2);
        Player.coins -= upgradePrice;

        upgradePrice = (float)Math.Round(startPrice * 2, 1);
        upgradeDamage = (float)Math.Round(damage * 1.2, 1);
        upgradeAttackSpeed = (float)Math.Round(attackSpeed * 1.2, 1);
        upgradeRange = (float)Math.Round(range * 1.2, 1);
    }

    private void UpdateRange(){
        float diameter = range / 100;
        rangePreview.transform.localScale = new Vector3(diameter, diameter, 1);

    }

    public void DealDamage(GameObject enemy)
    {
        Enemy enemyHealth = enemy.GetComponent<Enemy>();
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(this.damage);
        }
    }

    public void EnemyEnteredRange(GameObject enemy)
    {
        enemiesInRange.Add(enemy);
        DealDamage(enemy);
    }

    public void EnemyExitedRange(GameObject enemy)
    {
        enemiesInRange.Remove(enemy);
    }
}
