using UnityEngine;
using TMPro;

public class Enemy : MonoBehaviour
{
    public float maxHealth = 100;
    public int coinsWhenDied = 1;
    public float speed = 2;
    public RectTransform healthBarForeground;
    public TextMeshProUGUI damageText;

    private float originalHealthBarWidth;
    private float currentHealth;
    private SpriteRenderer spriteRenderer;

    private AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    public void Start()
    {
        currentHealth = maxHealth;
        originalHealthBarWidth = healthBarForeground.sizeDelta.x;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(float damage)
    {
        if (damage <= 0)
        {
            return;
        }
        
        DimSprite();
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);

        Debug.Log(this.name + " took " + damage + " damage. Health left: " + currentHealth);
        UpdateHealthBar();
        DisplayDamage(damage);

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
        audioManager.PlaySFX(audioManager.virusDeath); // ------------------------------------------------------------------------------------------------------------------
        Destroy(gameObject);
        EnemyGenerator.EnemyCount--;
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

    void DisplayDamage(float damage) 
    {
        damageText.enabled = true;
        damageText.text = "-" + damage.ToString();
        Invoke(nameof(DisableDamageText), 0.5f);
    }

    void DisableDamageText()
    {
        damageText.enabled = false;
    }

    public bool IsAlive() {
        if (currentHealth > 0)
        {
            return true;
        }

        return false;
    }
}
