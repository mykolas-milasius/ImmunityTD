using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public float damage;
    public float attackSpeed;
    public float range;
    public float startPrice;

    public TextMeshProUGUI damageText;
    public TextMeshProUGUI attackSpeedText;
    public TextMeshProUGUI rangeText;
    public GameObject rangePreview;

    private List<GameObject> enemiesInRange = new List<GameObject>();

    public void Start()
    {
        damageText.text = damage.ToString();
        attackSpeedText.text = attackSpeed.ToString();
        rangeText.text = range.ToString();
        UpdateRange();
    }

    public void Update(){
        if (enemiesInRange.Count > 0) {
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
