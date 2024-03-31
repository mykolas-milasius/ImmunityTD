using UnityEngine;
using TMPro;

public class Enemy : MonoBehaviour
{
    public float maxHealth = 100;
    public float coinsWhenDied = 1;
    public float speed = 2;
    public RectTransform healthBarForeground;

    private float originalHealthBarWidth;
    private float currentHealth;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        currentHealth = maxHealth;
        originalHealthBarWidth = healthBarForeground.sizeDelta.x;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(float damage)
    {
        DimSprite();
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);

        Debug.Log(this.name + " took " + damage + " damage. Health left: " + currentHealth);
        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Player.AddCoins(coinsWhenDied);
        Player.AddKill();
        Player.AddScore(coinsWhenDied);
        Destroy(gameObject);
    }

    void UpdateHealthBar()
    {
        float healthRatio = currentHealth / maxHealth;
        healthBarForeground.sizeDelta = new Vector2(originalHealthBarWidth * healthRatio, healthBarForeground.sizeDelta.y);
    }

    void DimSprite()
    {
        if (spriteRenderer != null)
        {
            Color newColor = spriteRenderer.color;
            newColor.r = 0.8f;
            spriteRenderer.color = newColor;

            Invoke(nameof(ResetSpriteColor), 0.05f);
        }
    }

    void ResetSpriteColor()
    {
        if (spriteRenderer != null)
        {
            Color originalColor = spriteRenderer.color;
            originalColor.r = 1f;
            spriteRenderer.color = originalColor;
        }
    }
}
