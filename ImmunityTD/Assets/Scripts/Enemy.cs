using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float maxHealth = 100;
    public float scoreWhenDied = 10;
    public float speed = 2;
    public RectTransform healthBarForeground;
    private float originalHealthBarWidth;
    private float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        originalHealthBarWidth = healthBarForeground.sizeDelta.x;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);
        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    void UpdateHealthBar()
    {
        float healthRatio = currentHealth / maxHealth;
        healthBarForeground.sizeDelta = new Vector2(originalHealthBarWidth * healthRatio, healthBarForeground.sizeDelta.y);
    }
}
