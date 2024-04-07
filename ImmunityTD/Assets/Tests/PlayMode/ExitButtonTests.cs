using NUnit.Framework;
using UnityEngine;

public class ExitButtonTests
{
    private GameObject exitGameObject;
    private Exit exitScript;
    private GameObject quitPanel;

    // Setup for each test
    [SetUp]
    public void Setup()
    {
        // Create a new GameObject for the Exit script and add the Exit component
        exitGameObject = new GameObject("ExitButton");
        exitScript = exitGameObject.AddComponent<Exit>();

        // Create a new GameObject for the quit panel and assign it to the Exit script
        quitPanel = new GameObject("QuitPanel");
        exitScript.quitPanel = quitPanel;

        // Simulate starting conditions if necessary
        quitPanel.SetActive(false); // Assuming the quit panel starts inactive
    }

    // Test to check if the confirmation menu opens correctly
    [Test]
    public void OpenConfirmationMenu_ActivatesQuitPanel()
    {
        // Activate the confirmation menu
        exitScript.openConfirmationMenu();

        // Assert that the quit panel is active
        Assert.IsTrue(quitPanel.activeSelf, "Quit panel should be active after opening confirmation menu");
    }

    // Test to check if the confirmation menu closes correctly
    [Test]
    public void CloseConfirmationMenu_DeactivatesQuitPanel()
    {
        // First, open the confirmation menu to activate the quit panel
        exitScript.openConfirmationMenu();
        // Now, close the confirmation menu
        exitScript.closeConfirmationMenu();

        // Assert that the quit panel is no longer active
        Assert.IsFalse(quitPanel.activeSelf, "Quit panel should be inactive after closing confirmation menu");
    }

    // Cleanup after each test
    [TearDown]
    public void Teardown()
    {
        // Destroy the GameObjects to clean up the environment for the next test
        Object.DestroyImmediate(exitGameObject);
        Object.DestroyImmediate(quitPanel);
    }
}
