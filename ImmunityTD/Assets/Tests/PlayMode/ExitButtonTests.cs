using NUnit.Framework;
using UnityEngine;

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
        exitScript.quitPanel = quitPanel;

        quitPanel.SetActive(false);
    }

    [Test]
    public void OpenConfirmationMenu_ActivatesQuitPanel()
    {
        exitScript.openConfirmationMenu();
        Assert.IsTrue(quitPanel.activeSelf, "Quit panel should be active after opening confirmation menu");
    }
    [Test]
    public void CloseConfirmationMenu_DeactivatesQuitPanel()
    {
        exitScript.openConfirmationMenu();
        exitScript.closeConfirmationMenu();

        Assert.IsFalse(quitPanel.activeSelf, "Quit panel should be inactive after closing confirmation menu");
    }

    [Test]
    public void OpenConfirmationMenu_WhenAlreadyOpen_KeepsQuitPanelActive()
    {
        exitScript.openConfirmationMenu();
        exitScript.openConfirmationMenu();
        Assert.IsTrue(quitPanel.activeSelf, "Quit panel should remain active when openConfirmationMenu is called while it's already open");
    }

    [Test]
    public void CloseConfirmationMenu_WhenAlreadyClosed_KeepsQuitPanelInactive()
    {
        quitPanel.SetActive(false);
        exitScript.closeConfirmationMenu();

        Assert.IsFalse(quitPanel.activeSelf, "Quit panel should remain inactive when closeConfirmationMenu is called while it's already closed");
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
        exitScript.applicationQuitter = mockQuitter;

        exitScript.QuitGame();

        Assert.IsTrue(mockQuitter.quitCalled, "QuitGame should call Quit on applicationQuitter");
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
