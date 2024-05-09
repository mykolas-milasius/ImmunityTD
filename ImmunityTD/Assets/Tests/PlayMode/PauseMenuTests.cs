using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;
using Assets.Scripts;

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
        pauseMenuScript.PauseMenuObject = new GameObject("PauseMenuPanel");
        pauseMenuScript.PauseButton = new GameObject("PauseButton");

        // Initially, the pause menu is inactive and the pause Button is active
        pauseMenuScript.PauseMenuObject.SetActive(false);
        pauseMenuScript.PauseButton.SetActive(true);
    }

    [Test]
    public void PauseGame_ActivatesPauseMenu()
    {
        // Act
        pauseMenuScript.PauseGame();

        // Assert
        Assert.IsTrue(pauseMenuScript.IsPaused);
        Assert.AreEqual(0, Time.timeScale);
        Assert.IsTrue(pauseMenuScript.PauseMenuObject.activeSelf);
        Assert.IsFalse(pauseMenuScript.PauseButton.activeSelf);
    }

    [Test]
    public void ResumeGame_DeactivatesPauseMenu()
    {
        // Set up initial pause state
        pauseMenuScript.PauseGame();

        // Act
        pauseMenuScript.ResumeGame();

        // Assert
        Assert.IsFalse(pauseMenuScript.IsPaused);
        Assert.AreEqual(1, Time.timeScale);
        Assert.IsFalse(pauseMenuScript.PauseMenuObject.activeSelf);
        Assert.IsTrue(pauseMenuScript.PauseButton.activeSelf);
    }

    [Test]
    public void PauseAndResumeGame_TogglesPauseState()
    {
        // Act & Assert
        pauseMenuScript.PauseGame();
        Assert.IsTrue(pauseMenuScript.IsPaused);

        pauseMenuScript.ResumeGame();
        Assert.IsFalse(pauseMenuScript.IsPaused);
    }

    [Test]
    public void TogglePause_TogglesPauseStateAndUIElements()
    {
        // Initially, the game is not paused
        Assert.IsFalse(pauseMenuScript.IsPaused);
        Assert.IsTrue(pauseMenuScript.PauseButton.activeSelf);
        Assert.IsFalse(pauseMenuScript.PauseMenuObject.activeSelf);

        // Act - Simulate the first toggle (should pause the game)
        pauseMenuScript.TogglePause();

        // Assert - The game should now be paused
        Assert.IsTrue(pauseMenuScript.IsPaused);
        Assert.IsFalse(pauseMenuScript.PauseButton.activeSelf);
        Assert.IsTrue(pauseMenuScript.PauseMenuObject.activeSelf);

        // Act - Simulate the second toggle (should resume the game)
        pauseMenuScript.TogglePause();

        // Assert - The game should now be resumed
        Assert.IsFalse(pauseMenuScript.IsPaused);
        Assert.IsTrue(pauseMenuScript.PauseButton.activeSelf);
        Assert.IsFalse(pauseMenuScript.PauseMenuObject.activeSelf);
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
        Assert.IsTrue(pauseMenuScript.IsPaused);
        Assert.IsTrue(pauseMenuScript.PauseMenuObject.activeSelf);
        Assert.IsFalse(pauseMenuScript.PauseButton.activeSelf);

        // Reset and simulate another Escape key press
        mockInputHandler.EscapePressed = false; // Reset the key press state
        yield return null; // Wait one frame to process the key release
        mockInputHandler.EscapePressed = true; // Simulate the key press again

        // Wait one frame to let Update method process the input again
        yield return null;

        // Assert the game is resumed
        Assert.IsFalse(pauseMenuScript.IsPaused);
        Assert.IsFalse(pauseMenuScript.PauseMenuObject.activeSelf);
        Assert.IsTrue(pauseMenuScript.PauseButton.activeSelf);
    }


    [TearDown]
    public void TearDown()
    {
        // Reset the time scale to its original value
        Time.timeScale = originalTimeScale;

        // Clean up the game objects created for the tests
        GameObject.DestroyImmediate(pauseMenuScript.PauseMenuObject);
        GameObject.DestroyImmediate(pauseMenuScript.PauseButton);
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
