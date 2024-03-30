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
    private List<GameObject> enemiesInRange = new List<GameObject>();

    public void Start()
    {
        damageText.text = damage.ToString();
        attackSpeedText.text = attackSpeed.ToString();
        rangeText.text = range.ToString();
    }

    public void Update(){
        if (enemiesInRange.Count > 0) {
            DealDamage(enemiesInRange[0]);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemiesInRange.Add(other.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemiesInRange.Remove(other.gameObject);
        }
    }

    public void DealDamage(GameObject enemy)
    {
        EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(this.damage);
        }
    }
}
