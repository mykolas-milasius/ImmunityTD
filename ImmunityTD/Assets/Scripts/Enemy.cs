using UnityEngine;
using TMPro;

namespace Assets.Scripts
{
    public class Enemy : MonoBehaviour
    {
        public string Name = "Virus";
        public bool IsVirus = true;
        public float MaxHealth = 100;
        public int CoinsWhenDied = 1;
        public float Speed = 2;
        public int DamageIfNotKilled = 1;
        public RectTransform HealthBarForeground;

        private float _originalHealthBarWidth;
        private bool _effectUsed = false;
        private float _currentHealth;
        private float _originalSpeed;
        private SpriteRenderer _spriteRenderer;
        private AudioManager _audioManager;

        public TextMeshProUGUI DamageText;

        #region Getters and Setters

        public float GetSpeed()
        {
            return Speed;
        }

        public int GetDamageIfNotKilled()
        {
            return DamageIfNotKilled;
        }

        #endregion

        private void Awake()
        {
            _audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        }

        public void Start()
        {
            _currentHealth = MaxHealth;
            _originalHealthBarWidth = HealthBarForeground.sizeDelta.x;
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        #region Collision Detection

        public void TakeDamage(float damage)
        {
            if (damage <= 0)
            {
                return;
            }

            DimSprite();
            _currentHealth -= damage;
            _currentHealth = Mathf.Max(_currentHealth, 0);

            Debug.Log(Name + " took " + damage + " Damage. Health left: " + _currentHealth);
            UpdateHealthBar();
            DisplayDamage(damage);

            UseSpecialEffect();

            if (_currentHealth <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            Player.AddCoins(CoinsWhenDied);
            Player.AddKill();
            Player.AddScore(CoinsWhenDied);
            _audioManager.PlaySFX(_audioManager.VirusDeath);
            Destroy(gameObject);
            EnemyGenerator.EnemyCount--;
        }

        private void UpdateHealthBar()
        {
            var healthRatio = _currentHealth / MaxHealth;
            HealthBarForeground.sizeDelta =
                new Vector2(_originalHealthBarWidth * healthRatio, HealthBarForeground.sizeDelta.y);
        }

        private void DisplayDamage(float damage)
        {
            DamageText.enabled = true;
            DamageText.text = "-" + damage;
            Invoke(nameof(DisableDamageText), 0.5f);
        }

        private void DisableDamageText()
        {
            DamageText.enabled = false;
        }

        public bool IsAlive()
        {

            return _currentHealth > 0;
        }

        #endregion

        #region Special Effects

        private void UseSpecialEffect()
        {
            if (_effectUsed)
            {
                return;
            }

            switch (Name)
            {
                case "Rabies":
                    DoubleSpeed();
                    break;
            }
        }

        private void DoubleSpeed()
        {
            if (_currentHealth < 0 && _currentHealth > MaxHealth * 0.5)
            {
                return;
            }

            _originalSpeed = Speed;
            Speed *= 2;
            _effectUsed = true;
            MakeSpriteTransparent();
            Debug.Log($"{Name} effect used: Double Speed. Speed: {Speed}");
            Invoke("ResetSpeed", 1f);
        }

        private void ResetSpeed()
        {
            Speed = _originalSpeed;
            ResetSpriteTransparency();
            Debug.Log($"{Name} effect ended. Speed: {Speed}");
        }


        #endregion

        #region Sprite Manipulation

        private void MakeSpriteTransparent()
        {
            if (_spriteRenderer != null)
            {
                Color newColor = _spriteRenderer.color;
                newColor.a = 0.5f;
                _spriteRenderer.color = newColor;
            }
        }

        private void ResetSpriteTransparency()
        {
            if (_spriteRenderer != null)
            {
                Color originalColor = _spriteRenderer.color;
                originalColor.a = 1f;
                _spriteRenderer.color = originalColor;
            }
        }

        private void DimSprite()
        {
            if (_spriteRenderer != null)
            {
                Color newColor = _spriteRenderer.color;
                newColor.r = 0.8f;
                _spriteRenderer.color = newColor;

                Invoke(nameof(ResetSpriteColor), 0.05f);
            }
        }

        private void ResetSpriteColor()
        {
            if (_spriteRenderer == null)
            {
                return;
            }

            var originalColor = _spriteRenderer.color;
            originalColor.r = 1f;
            _spriteRenderer.color = originalColor;
        }

        #endregion
    }
}
