using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Reflection;
using System.Collections;
using UnityEngine.TestTools;

public class EnemyGeneratorTests
{
    private GameObject generatorGO;
    private EnemyGenerator generator;
    private string aidsPrefabPath = "Prefabs/Enemies/AIDSVirus";
    private string testSceneName; // Store the scene name for use in TearDown

    [SetUp]
    public void SetUp()
    {
        // Generate a unique scene name to avoid conflicts
        testSceneName = "TestScene" + System.Guid.NewGuid().ToString();

        // Create a new scene with a unique name for each test
        SceneManager.CreateScene(testSceneName);

        generatorGO = new GameObject("EnemyGenerator");
        generator = generatorGO.AddComponent<EnemyGenerator>();

        // Ensure the prefab is located in a Resources folder and the path is correct
        generator.enemyPrefab = Resources.Load<GameObject>(aidsPrefabPath);
        Assert.IsNotNull(generator.enemyPrefab, "Failed to load Enemy prefab. Ensure the path is correct and the prefab exists in a Resources folder.");

        generator.wayPoints = new GameObject("Waypoints");
        for (int i = 0; i < 5; i++)
        {
            GameObject wp = new GameObject($"WP{i}");
            wp.transform.parent = generator.wayPoints.transform;
        }
    }


    [Test]
    public void RandomInterval_SetsSpawnIntervalWithinRange()
    {
        // Use reflection to get the RandomInterval method
        MethodInfo randomIntervalMethod = typeof(EnemyGenerator).GetMethod("RandomInterval", BindingFlags.NonPublic | BindingFlags.Instance);

        // Invoke the RandomInterval method
        randomIntervalMethod.Invoke(generator, null);

        // Assert that spawnInterval is within the expected range
        Assert.IsTrue(generator.spawnInterval >= 0.1f && generator.spawnInterval <= 3f);
    }

    [Test]
    public void SpawnEnemy_CreatesEnemyAndAssignsWaypoints()
    {
        generator.SpawnEnemy();

        // Check if an enemy was spawned
        var enemy = GameObject.FindWithTag("Enemy");
        Assert.IsNotNull(enemy);

        // Check if the enemy has the correct waypoints assigned
        var enemyPath = enemy.GetComponent<EnemyPath>();
        Assert.IsNotNull(enemyPath);
        Assert.AreEqual(generator.wayPoints.GetComponentsInChildren<Transform>().Length - 1, enemyPath.waypoints.Length);
    }

    [UnityTest]
    public IEnumerator Update_SpawnsEnemyAfterInterval()
    {
        float initialInterval = generator.spawnInterval;
        yield return new WaitForSeconds(initialInterval + 0.1f); // Wait for spawnInterval + a little more

        // Check if an enemy was spawned
        var enemy = GameObject.FindWithTag("Enemy");
        Assert.IsNotNull(enemy);
    }

    [Test]
    public void SpawnEnemy_ResetsTimer()
    {
        // Spawn an enemy to reset the timer
        generator.SpawnEnemy();

        // Use reflection to get the private timer field
        FieldInfo timerField = typeof(EnemyGenerator).GetField("timer", BindingFlags.NonPublic | BindingFlags.Instance);
        float timerValue = (float)timerField.GetValue(generator);

        // Assert that timer is reset to 0
        Assert.AreEqual(0f, timerValue);
    }

    [Test]
    public void Start_SetsRandomSpawnInterval()
    {
        // Call Start manually to simulate the behaviour
        generator.Start();

        // Assert that spawnInterval is within the expected range
        Assert.IsTrue(generator.spawnInterval >= 0.1f && generator.spawnInterval <= 3f);
    }

    [Test]
    public void SpawnEnemy_InvokesRandomIntervalAgain()
    {
        // Record the initial spawn interval
        float initialInterval = generator.spawnInterval;

        // Spawn an enemy, which should invoke RandomInterval again
        generator.SpawnEnemy();

        // Assert that the spawn interval has changed
        Assert.AreNotEqual(initialInterval, generator.spawnInterval);
    }

    [TearDown]
    public void TearDown()
    {
        // Destroy the test objects after each test
        if (generatorGO != null)
            GameObject.DestroyImmediate(generatorGO);

        // Use the stored scene name to unload the scene
        if (SceneManager.GetSceneByName(testSceneName).IsValid())
            SceneManager.UnloadSceneAsync(testSceneName);
    }
}
