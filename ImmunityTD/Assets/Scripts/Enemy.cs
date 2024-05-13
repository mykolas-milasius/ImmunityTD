using System.Collections;
using UnityEngine;
using TMPro;

namespace Assets.Scripts
{
    public class Enemy : MonoBehaviour
    {
        // ReSharper disable FieldCanBeMadeReadOnly.Global MemberCanBePrivate.Global ConvertToConstant.Global RedundantDefaultMemberInitializer
        public string Name = "Virus";
        public bool IsVirus = true; // if false, it's a bacterium
        public float MaxHealth = 100;
        public int CoinsWhenDied = 1;
        public float Speed = 2;
        public int DamageIfNotKilled = 1;

        public RectTransform HealthBarForeground;
        public TextMeshProUGUI DamageText;

        private float _originalHealthBarWidth;
        private bool _effectUsed = false;
        private float _currentHealth;
        private float _originalSpeed;
        private bool _isInvisible = false;
        
        private SpriteRenderer _spriteRenderer;
        private AudioManager _audioManager;
        // ReSharper enable FieldCanBeMadeReadOnly.Global MemberCanBePrivate.Global ConvertToConstant.GlobalRedundantDefaultMemberInitializer

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
        
        // ReSharper disable once UnusedMember.Local
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

        public void Update()
        {
            // if (!_effectUsed)
            // {
            //     UseSpecialEffect(false);
            // }
        }

        #region Collision Detection

        public void TakeDamage(float damage)
        {
            if (damage <= 0)
            {
                return;
            }

            // Resists to damage by 20%
            if (Name == "Staphylococcus aureus")
            {
                damage *= 0.8f;
            }

            DimSprite();
            _currentHealth -= damage;
            _currentHealth = Mathf.Max(_currentHealth, 0);

            Debug.Log(Name + " took " + damage + " Damage. Health left: " + _currentHealth);
            UpdateHealthBar();
            DisplayDamage(damage);

            UseSpecialEffect(true);

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

        private void UseSpecialEffect(bool onHit)
        {
            if (_effectUsed)
            {
                return;
            }

            if (onHit)
            {
                switch (Name)
                {
                    case "Rabies":
                        IncreaseSpeed(2f, true);
                        Debug.Log("Rabies speed increased by 100%");
                        break;
                }
            }
            else
            {
                switch (Name)
                {
                    case "Tuberculosis":
                        if (!_effectUsed)
                        {
                            StartCoroutine(IncreaseDamageToPlayer(1, 5f));
                        }
                        break;
                    case "Influenza":
                        if (!_effectUsed)
                        {
                            StartCoroutine(IncreaseSpeedOverTime(1.1f, 5f));
                        }
                        break;
                    case "Papillomaviridae":
                        if (!_effectUsed)
                        {
                            MakeInvisible(0.3f, 1); 
                        }
                        break;

                }
            }
        }

        private void IncreaseSpeed(float percentage, bool reset)
        {
            if (_currentHealth < 0 && _currentHealth > MaxHealth * 0.5)
            {
                return;
            }

            _originalSpeed = Speed;
            Speed *= percentage;
            _effectUsed = true;
            MakeSpriteTransparent(0.6f);
            if (reset)
            {
                Invoke("ResetSpeed", 1f);
            }
        }

        // ReSharper disable once UnusedMember.Local
        private void ResetSpeed()
        {
            Speed = _originalSpeed;
            ResetSpriteTransparency();
        }

        private IEnumerator IncreaseSpeedOverTime(float percentage, float delay)
        {
            _effectUsed = true;
            if (_spriteRenderer)
            {
                while (true)
                {
                    yield return new WaitForSeconds(delay);
                    IncreaseSpeed(percentage, false);
                    Debug.Log($"{Name} speed increased by 10% every 5 seconds. Current speed: " + Speed);
                }
            }
        }

        private void MakeInvisible(float transparency, float duration )
        {
            if (_isInvisible)
            {
                return;
            }

            _effectUsed = true;

            while (true)
            {
                _isInvisible = true;
                MakeSpriteTransparent(transparency);
                Debug.Log($"{Name} made invisible (transparency: {transparency}) for 1 second.");
                Invoke(nameof(ResetSpriteTransparency), duration);
            }
        }
        
        private IEnumerator IncreaseDamageToPlayer(int damage, float delay)
        {
            _effectUsed = true;
            while (true)
            {
                DamageIfNotKilled += damage;
                yield return new WaitForSeconds(delay);
                Debug.Log($"{Name} damage increased by 1 every 5 seconds. Current damage: " + DamageIfNotKilled);
            }
            // ReSharper disable once IteratorNeverReturns
        }

        #endregion

        #region Sprite Manipulation

        private void MakeSpriteTransparent(float transparency = 0.5f)
        {
            if (_spriteRenderer != null)
            {
                Color newColor = _spriteRenderer.color;
                newColor.a = transparency;
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
                _isInvisible = false;
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