using System;
using System.Collections;
using System.Globalization;
using TMPro;
using UnityEngine;

namespace Assets.Scripts
{
    public class Player : MonoBehaviour
    {
        public static Player Instance; // Used to access Player class from other scripts
        public static float Coins = 100f;
        public static int Score = 0;
        public static int Kills = 0;
        public float GeneratorDelay = 8f;
        public int Difficulty = 2; // 1 - easy, 2 - medium, 3 - hard, 4 - slavery
        public bool DebugMode = false; // No delay for enemy generator, infinite coins
        public bool EndlessMode = false;
        private static int _health = 100;

        private float _timer = 0f;
        private readonly float _enemyWaveTextTimer = 5f;
        private bool _gameRunning = false;
        private bool _gameFinished = false;

        public GameObject EnemyGenerator;
        public TextMeshProUGUI CoinsText;
        public TextMeshProUGUI ScoreText;
        public TextMeshProUGUI KillsText;
        public TextMeshProUGUI HealthText;
        public TextMeshProUGUI EnemySpawnDelayText;
        public TextMeshProUGUI EnemyWaveText;
        public TextMeshProUGUI GameOverText;

        public void FixedUpdate()
        {
            if (!_gameFinished && !_gameRunning)
            {
                StartEnemyGenerator();
            }

            UpdateTaskbar();
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public static void AddCoins(float coinsToAdd)
        {
            Coins += coinsToAdd;
        }

        public static void AddScore(int scoreToAdd)
        {
            Score += scoreToAdd;
        }

        public static void AddKill()
        {
            Kills++;
        }

        public static void AddKills(int killsToAdd)
        {
            Kills += killsToAdd;
        }

        public static void TakeDamage(int damage)
        {
            _health -= damage;
            if (_health <= 0)
            {
                Instance.FinishGame();
            }
        }

        void StartEnemyGenerator()
        {
            EnemySpawnDelayText.gameObject.SetActive(true);

            if (!DebugMode)
            {
                if (_timer < GeneratorDelay)
                {
                    _timer += Time.deltaTime;
                    EnemySpawnDelayText.text =
                        String.Format("Enemies spawn in: {0:f1} seconds", GeneratorDelay - _timer);
                }
                else if (!_gameRunning)
                {
                    TurnOnGame();
                }
            }
            else
            {
                Coins = 100000000;
                GeneratorDelay = 1f;
                Difficulty = 4;
                TurnOnGame();
            }
        }

        void UpdateTaskbar()
        {
            NumberFormatInfo nfi = new NumberFormatInfo
            {
                NumberGroupSeparator = " ",
                NumberGroupSizes = new int[] { 3 }
            };

            if (CoinsText != null)
            {
                CoinsText.text = Coins.ToString("n0", nfi);
            }

            if (ScoreText != null)
            {
                ScoreText.text = Score.ToString("n0", nfi);
            }

            if (KillsText != null)
            {
                KillsText.text = Kills.ToString("n0", nfi);
            }

            if (HealthText != null)
            {
                HealthText.text = _health.ToString("n0", nfi);
            }
        }

        public void TurnOnGame()
        {
            if (!_gameRunning)
            {
                _gameRunning = true;
                EnemyGenerator.SetActive(true);
                EnemySpawnDelayText.gameObject.SetActive(false);
                EnemyGenerator.GetComponent<EnemyGenerator>().StartNewWave();
                Debug.Log("Game started");
            }
        }

        public void TurnOffGame()
        {
            if (_gameRunning)
            {
                _gameRunning = false;
                _timer = 0f;
                EnemyGenerator.SetActive(false);
                EnemySpawnDelayText.gameObject.SetActive(true);
                Debug.Log("Game stopped");
            }
        }

        public void FinishGame()
        {
            if (_gameRunning)
            {
                _gameRunning = false;
                _gameFinished = true;
                _timer = 0f;
                EnemyGenerator.SetActive(false);
                EnemySpawnDelayText.gameObject.SetActive(false);
                GameOverText.gameObject.SetActive(true);
                GameOverText.text = "Game Over";
                Debug.Log("Game finished");
            }
        }

        public void UpdateEnemyWaveText(int wave, int maxWaves, int enemiesInWave)
        {
            if (EnemyWaveText != null)
            {
                EnemyWaveText.gameObject.SetActive(true);
                EnemyWaveText.text = $"Wave {wave}/{maxWaves}. Enemies in wave: {enemiesInWave}";
                StartCoroutine(HideEnemyWaveTextAfterDelay(_enemyWaveTextTimer));
            }
        }

        private IEnumerator HideEnemyWaveTextAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            if (EnemyWaveText != null)
            {
                EnemyWaveText.gameObject.SetActive(false);
            }
        }
    }
}