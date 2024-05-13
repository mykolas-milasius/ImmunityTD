using UnityEngine.TestTools;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Assets.Scripts;

public class LoadSceneTests
{
    private GameObject gameObject;
    private LoadScene loadScene;

    [SetUp]
    public void SetUp()
    {
        gameObject = new GameObject();
        loadScene = gameObject.AddComponent<LoadScene>();
    }

    [TearDown]
    public void TearDown()
    {
        Object.Destroy(gameObject);
    }

    [UnityTest]
    public IEnumerator LoadScene_LoadsCorrectScene()
    {
        string sceneName = "ExampleScene";

        loadScene.Load(sceneName);
        yield return null;

        Scene loadedScene = SceneManager.GetSceneByName(sceneName);
        Assert.IsTrue(loadedScene.IsValid(), "Scene was not loaded.");
        Assert.IsTrue(loadedScene.isLoaded, "Scene was not loaded.");
    }
}
