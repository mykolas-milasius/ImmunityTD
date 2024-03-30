using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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

    private List<GameObject> enemiesInRange = new List<GameObject>();
    private float attackCooldown;

    public void Start()
    {
        damageText.text = damage.ToString();
        attackSpeedText.text = attackSpeed.ToString();
        rangeText.text = range.ToString();
        UpdateRange();
        attackCooldown = 1f / attackSpeed;
    }

    public void Update(){
        attackCooldown -= Time.deltaTime;

        if (attackCooldown <= 0f && enemiesInRange.Count > 0)
        {
            attackCooldown = 1f / attackSpeed;
            DealDamage(enemiesInRange[0]);
        }
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
