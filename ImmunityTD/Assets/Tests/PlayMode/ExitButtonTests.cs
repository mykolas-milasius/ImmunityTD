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

    [Test]
    public void OpenConfirmationMenu_WhenAlreadyOpen_KeepsQuitPanelActive()
    {
        // First, open the confirmation menu to activate the quit panel
        exitScript.openConfirmationMenu();
        // Call openConfirmationMenu again
        exitScript.openConfirmationMenu();

        // Assert that the quit panel remains active
        Assert.IsTrue(quitPanel.activeSelf, "Quit panel should remain active when openConfirmationMenu is called while it's already open");
    }

    [Test]
    public void CloseConfirmationMenu_WhenAlreadyClosed_KeepsQuitPanelInactive()
    {
        // Ensure the quit panel is initially inactive
        quitPanel.SetActive(false);
        // Call closeConfirmationMenu on an already inactive quit panel
        exitScript.closeConfirmationMenu();

        // Assert that the quit panel remains inactive
        Assert.IsFalse(quitPanel.activeSelf, "Quit panel should remain inactive when closeConfirmationMenu is called while it's already closed");
    }

    [Test]
    public void QuitPanel_InitialState()
    {
        // Assuming the quit panel should be initially inactive, as set in the Setup method
        Assert.IsFalse(quitPanel.activeSelf, "Quit panel should be inactive at the start");
    }

    [Test]
    public void QuitGame_CallsApplicationQuit()
    {
        var mockQuitter = new MockApplicationQuitter();
        exitScript.applicationQuitter = mockQuitter;

        exitScript.QuitGame();

        Assert.IsTrue(mockQuitter.quitCalled, "QuitGame should call Quit on applicationQuitter");
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

public class MockApplicationQuitter : IApplicationQuitter
{
    public bool quitCalled = false;

    public void Quit()
    {
        quitCalled = true;
    }
}
