using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Reflection;

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
