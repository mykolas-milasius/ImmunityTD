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
        SetupRigidbody();
    }

    public void TakeDamage(float damage)
    {
        Debug.Log(this + "took damage");
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

    private void SetupRigidbody()
{
    Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();
    if (rb == null)
    {
        rb = gameObject.AddComponent<Rigidbody2D>();
    }
    
    rb.bodyType = RigidbodyType2D.Kinematic;
    rb.gravityScale = 0;
    rb.interpolation = RigidbodyInterpolation2D.Interpolate;
    rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
}
}
