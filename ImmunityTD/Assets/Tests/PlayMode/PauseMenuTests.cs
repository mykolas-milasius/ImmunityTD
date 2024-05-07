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

        // Initially, the pause menu is inactive and the pause Button is active
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

    [Test]
    public void TogglePause_TogglesPauseStateAndUIElements()
    {
        // Initially, the game is not paused
        Assert.IsFalse(pauseMenuScript.isPaused);
        Assert.IsTrue(pauseMenuScript.pauseButton.activeSelf);
        Assert.IsFalse(pauseMenuScript.pauseMenu.activeSelf);

        // Act - Simulate the first toggle (should pause the game)
        pauseMenuScript.TogglePause();

        // Assert - The game should now be paused
        Assert.IsTrue(pauseMenuScript.isPaused);
        Assert.IsFalse(pauseMenuScript.pauseButton.activeSelf);
        Assert.IsTrue(pauseMenuScript.pauseMenu.activeSelf);

        // Act - Simulate the second toggle (should resume the game)
        pauseMenuScript.TogglePause();

        // Assert - The game should now be resumed
        Assert.IsFalse(pauseMenuScript.isPaused);
        Assert.IsTrue(pauseMenuScript.pauseButton.activeSelf);
        Assert.IsFalse(pauseMenuScript.pauseMenu.activeSelf);
    }

    [UnityTest]
    public IEnumerator Update_TogglesPauseWhenEscapeIsPressed()
    {
        var mockInputHandler = new MockInputHandler();
        pauseMenuScript.SetInputHandlerForTesting(mockInputHandler);

        // Simulate Escape key press
        mockInputHandler.EscapePressed = true;

        // Wait one frame to let Update method process the input
        yield return null;

        // Assert the game is paused
        Assert.IsTrue(pauseMenuScript.isPaused);
        Assert.IsTrue(pauseMenuScript.pauseMenu.activeSelf);
        Assert.IsFalse(pauseMenuScript.pauseButton.activeSelf);

        // Reset and simulate another Escape key press
        mockInputHandler.EscapePressed = false; // Reset the key press state
        yield return null; // Wait one frame to process the key release
        mockInputHandler.EscapePressed = true; // Simulate the key press again

        // Wait one frame to let Update method process the input again
        yield return null;

        // Assert the game is resumed
        Assert.IsFalse(pauseMenuScript.isPaused);
        Assert.IsFalse(pauseMenuScript.pauseMenu.activeSelf);
        Assert.IsTrue(pauseMenuScript.pauseButton.activeSelf);
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

public class MockInputHandler : IInputHandler
{
    public bool EscapePressed { get; set; }

    public bool GetKeyDown(KeyCode key)
    {
        return key == KeyCode.Escape && EscapePressed;
    }
}
