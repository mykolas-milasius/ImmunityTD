using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class Tower : MonoBehaviour
    {
        // ReSharper disable FieldCanBeMadeReadOnly.Global MemberCanBePrivate.Global ConvertToConstant.Global RedundantDefaultMemberInitializer
        public float Damage;
        public float AttackSpeed; // attacks per second
        public float Range;
        public float StartPrice;
        public bool Entered;
        public bool Exited;
        
        public GameObject BulletPrefab;
        public GameObject RangePreview;
        public Button Button;
        
        public TextMeshProUGUI DamageText;
        public TextMeshProUGUI AttackSpeedText;
        public TextMeshProUGUI RangeText;

        public TextMeshProUGUI UpgradeDamageText;
        public TextMeshProUGUI UpgradeAttackSpeedText;
        public TextMeshProUGUI UpgradeRangeText;
        public TextMeshProUGUI UpgradePriceText;
        
        private List<GameObject> _enemiesInRange = new List<GameObject>();
        private AudioManager _audioManager;

        private float _attackCooldown;
        private float _upgradeDamage;
        private float _upgradeAttackSpeed;
        private float _upgradeRange;
        private float _upgradePrice;
        private bool _isDamageDecreased = false;
        // ReSharper enable FieldCanBeMadeReadOnly.Global MemberCanBePrivate.Global ConvertToConstant.Global RedundantDefaultMemberInitializer
        
        private void Awake()
        {
            _audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        }

        public void Start()
        {
            if (RangePreview == null || BulletPrefab == null)
            {
                // Debug.LogError("Missing references in Tower script. Ensure all required fields are assigned.");
                return;
            }

            _upgradePrice = (float)Math.Round(StartPrice * 2, 1);
            _upgradeDamage = (float)Math.Round(Damage * 1.2, 1);
            _upgradeAttackSpeed = (float)Math.Round(AttackSpeed * 1.2, 1);
            _upgradeRange = (float)Math.Round(Range * 1.2, 1);
            if (Button != null)
            {
                Button.interactable = false;
            }

            UpdateRange();
            _attackCooldown = 1f / AttackSpeed;
        }

        public void Update()
        {

            if (DamageText != null)
            {
                DamageText.text = Damage.ToString();
                AttackSpeedText.text = AttackSpeed.ToString();
                RangeText.text = Range.ToString();
            }
            else
            {
                // Debug.LogWarning("UI text fields not assigned in Tower script.");
            }

            if (UpgradeDamageText != null)
            {
                UpgradeDamageText.text = _upgradeDamage.ToString();
                UpgradeAttackSpeedText.text = _upgradeAttackSpeed.ToString();
                UpgradeRangeText.text = _upgradeRange.ToString();
                UpgradePriceText.text = _upgradePrice.ToString();
            }
            else
            {
                // Debug.LogWarning("Upgrade UI elements not assigned in Tower script.");
            }

            if (_enemiesInRange.Count != 0 && Button != null)
            {
                _attackCooldown -= Time.deltaTime;
                if (_attackCooldown <= 0f)
                {
                    Shoot();
                    _attackCooldown = 1f / AttackSpeed;
                }
            }

            if (Button != null)
            {
                if (Player.Coins >= _upgradePrice)
                {
                    Button.interactable = true;
                }
                else
                {
                    Button.interactable = false;
                }
            }

        }

        public void Upgrade()
        {
            Damage = (float)(Damage * 1.2);
            AttackSpeed = (float)(AttackSpeed * 1.2);
            Range = (float)(Range * 1.2);
            Player.Coins -= _upgradePrice;

            _upgradePrice = (float)Math.Round(_upgradePrice * 2, 1);
            _upgradeDamage = (float)Math.Round(Damage * 1.2, 1);
            _upgradeAttackSpeed = (float)Math.Round(AttackSpeed * 1.2, 1);
            _upgradeRange = (float)Math.Round(Range * 1.2, 1);
        }

        private void UpdateRange()
        {
            float diameter = (Range / 100) + 1;
            RangePreview.transform.localScale = new Vector3(diameter, diameter, 1);

        }
        
        #region Enemy Interaction

        public void Shoot()
        {
            GameObject bulletGO = Instantiate(BulletPrefab, transform.position, Quaternion.identity);
            Bullet bullet = bulletGO.GetComponent<Bullet>();

            _audioManager.PlaySFX(_audioManager.TowerShoot);

            if (bullet != null)
            {
                bullet.Tower = this;
                bullet.Seek(_enemiesInRange[0]);
            }
            else
            {
                Debug.LogWarning("Bullet prefab does not contain Bullet component.");
            }
        }

        public virtual void DealDamage(GameObject enemyGameObject)
        {
            Enemy enemy = enemyGameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                // Decrease damage dealt by 1% for 5 seconds
                if (enemy.Name == "Ebola")
                {
                    StartCoroutine(DecreaseDamageForDuration(0.01f,5, false));
                }
                // 
                if (enemy.Name == "AIDS")
                {
                    StartCoroutine(DecreaseDamageForDuration(0.3f, 3, true));
                }
                else
                {
                    enemy.TakeDamage(Damage);
                }
            }
        }

        public void EnemyEnteredRange(GameObject enemy)
        {
            _enemiesInRange.Add(enemy);
            Entered = true;
            Exited = false;
        }

        public void EnemyExitedRange(GameObject enemy)
        {
            _enemiesInRange.Remove(enemy);
            Entered = false;
            Exited = true;
        }
        
        private IEnumerator DecreaseDamageForDuration(float percentage, float delay, bool single)
        {
            float originalDamage = Damage;
            if (!_isDamageDecreased)
            {
                Damage *= (1 - percentage);
            }
            
            Debug.Log("Damage decreased by " + percentage * 100 + "%");
            yield return new WaitForSeconds(delay);

            _isDamageDecreased = single;
            Damage = originalDamage;
        }

        #endregion 
    }
}
