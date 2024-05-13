using NUnit.Framework;
using UnityEngine;
using Assets.Scripts;

public class ExitButtonTests
{
    private GameObject exitGameObject;
    private Exit exitScript;
    private GameObject quitPanel;

    [SetUp]
    public void Setup()
    {
        exitGameObject = new GameObject("ExitButton");
        exitScript = exitGameObject.AddComponent<Exit>();

        quitPanel = new GameObject("QuitPanel");
        exitScript.QuitPanel = quitPanel;

        quitPanel.SetActive(false);
    }

    [Test]
    public void OpenConfirmationMenu_ActivatesQuitPanel()
    {
        exitScript.OpenConfirmationMenu();
        Assert.IsTrue(quitPanel.activeSelf, "Quit panel should be active after opening confirmation menu");
    }
    [Test]
    public void CloseConfirmationMenu_DeactivatesQuitPanel()
    {
        exitScript.OpenConfirmationMenu();
        exitScript.CloseConfirmationMenu();

        Assert.IsFalse(quitPanel.activeSelf, "Quit panel should be inactive after closing confirmation menu");
    }

    [Test]
    public void OpenConfirmationMenu_WhenAlreadyOpen_KeepsQuitPanelActive()
    {
        exitScript.OpenConfirmationMenu();
        exitScript.OpenConfirmationMenu();
        Assert.IsTrue(quitPanel.activeSelf, "Quit panel should remain active when OpenConfirmationMenu is called while it's already open");
    }

    [Test]
    public void CloseConfirmationMenu_WhenAlreadyClosed_KeepsQuitPanelInactive()
    {
        quitPanel.SetActive(false);
        exitScript.CloseConfirmationMenu();

        Assert.IsFalse(quitPanel.activeSelf, "Quit panel should remain inactive when CloseConfirmationMenu is called while it's already closed");
    }

    [Test]
    public void QuitPanel_InitialState()
    {
        Assert.IsFalse(quitPanel.activeSelf, "Quit panel should be inactive at the start");
    }

    [Test]
    public void QuitGame_CallsApplicationQuit()
    {
        var mockQuitter = new MockApplicationQuitter();
        exitScript.ApplicationQuitter = mockQuitter;

        exitScript.QuitGame();

        Assert.IsTrue(mockQuitter.quitCalled, "QuitGame should call Quit on ApplicationQuitter");
    }

    [TearDown]
    public void Teardown()
    {
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
