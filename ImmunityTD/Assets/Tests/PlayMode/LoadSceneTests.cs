using UnityEngine.TestTools;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadSceneTests
{
    [UnityTest]
    public IEnumerator LoadScene_LoadsCorrectScene()
    {
        GameObject gameObject = new GameObject();
        LoadScene loadScene = gameObject.AddComponent<LoadScene>();
        string sceneName = "ExampleScene";

        loadScene.Load(sceneName);
        yield return null;

        Scene loadedScene = SceneManager.GetSceneByName(sceneName);
        Assert.IsTrue(loadedScene.IsValid(), "Scene was not loaded.");
        Assert.IsTrue(loadedScene.isLoaded, "Scene was not loaded.");
    }
}
