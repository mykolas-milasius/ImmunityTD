using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;

public class PauseMenuTests
{
    private GameObject pauseMenuGO;
    private PauseMenu pauseMenuScript;
    private float originalTimeScale;

    [SetUp]
    public void SetUp()
    {
        // Store the original time scale to reset it after tests
        originalTimeScale = Time.timeScale;

        // Create the game objects and components for the pause menu
        pauseMenuGO = new GameObject("PauseMenu");
        pauseMenuScript = pauseMenuGO.AddComponent<PauseMenu>();
        pauseMenuScript.pauseMenu = new GameObject("PauseMenuPanel");
        pauseMenuScript.pauseButton = new GameObject("PauseButton");

        // Initially, the pause menu is inactive and the pause button is active
        pauseMenuScript.pauseMenu.SetActive(false);
        pauseMenuScript.pauseButton.SetActive(true);
    }

    [Test]
    public void PauseGame_ActivatesPauseMenu()
    {
        // Act
        pauseMenuScript.PauseGame();

        // Assert
        Assert.IsTrue(pauseMenuScript.isPaused);
        Assert.AreEqual(0, Time.timeScale);
        Assert.IsTrue(pauseMenuScript.pauseMenu.activeSelf);
        Assert.IsFalse(pauseMenuScript.pauseButton.activeSelf);
    }

    [Test]
    public void ResumeGame_DeactivatesPauseMenu()
    {
        // Set up initial pause state
        pauseMenuScript.PauseGame();

        // Act
        pauseMenuScript.ResumeGame();

        // Assert
        Assert.IsFalse(pauseMenuScript.isPaused);
        Assert.AreEqual(1, Time.timeScale);
        Assert.IsFalse(pauseMenuScript.pauseMenu.activeSelf);
        Assert.IsTrue(pauseMenuScript.pauseButton.activeSelf);
    }

    [Test]
    public void PauseAndResumeGame_TogglesPauseState()
    {
        // Act & Assert
        pauseMenuScript.PauseGame();
        Assert.IsTrue(pauseMenuScript.isPaused);

        pauseMenuScript.ResumeGame();
        Assert.IsFalse(pauseMenuScript.isPaused);
    }

    [TearDown]
    public void TearDown()
    {
        // Reset the time scale to its original value
        Time.timeScale = originalTimeScale;

        // Clean up the game objects created for the tests
        GameObject.DestroyImmediate(pauseMenuScript.pauseMenu);
        GameObject.DestroyImmediate(pauseMenuScript.pauseButton);
        GameObject.DestroyImmediate(pauseMenuGO);
    }
}
