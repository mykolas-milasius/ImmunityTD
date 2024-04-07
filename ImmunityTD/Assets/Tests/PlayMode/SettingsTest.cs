using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class SettingsTest
{
    [UnityTest]
    public IEnumerator OpenSettingsMenu_SettingsMenuActive()
    {
        // Arrange
        GameObject pauseMenuObject = new GameObject();
        GameObject settingsMenuObject = new GameObject();
        SettingsScript settingsScript = pauseMenuObject.AddComponent<SettingsScript>();
        settingsScript.PauseMenu = pauseMenuObject;
        settingsScript.SettingsMenu = settingsMenuObject;

        // Act
        settingsScript.OpenSettingsMenu();
        yield return null; // Wait for one frame to let Unity execute the activation/deactivation of objects

        // Assert
        Assert.IsFalse(settingsScript.PauseMenu.activeSelf); // Pause menu should be inactive
        Assert.IsTrue(settingsScript.SettingsMenu.activeSelf); // Settings menu should be active
    }

    [UnityTest]
    public IEnumerator ReturnToPauseMenu_PauseMenuActive()
    {
        // Arrange
        GameObject pauseMenuObject = new GameObject();
        GameObject settingsMenuObject = new GameObject();
        SettingsScript settingsScript = pauseMenuObject.AddComponent<SettingsScript>();
        settingsScript.PauseMenu = pauseMenuObject;
        settingsScript.SettingsMenu = settingsMenuObject;

        // Act
        settingsScript.ReturnToPauseMenu();
        yield return null; // Wait for one frame to let Unity execute the activation/deactivation of objects

        // Assert
        Assert.IsTrue(settingsScript.PauseMenu.activeSelf); // Pause menu should be active
        Assert.IsFalse(settingsScript.SettingsMenu.activeSelf); // Settings menu should be inactive
    }
}
