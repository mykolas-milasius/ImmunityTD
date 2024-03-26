using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class ExitButtonTests
{
    [UnityTest]
    public IEnumerator OpenConfirmationMenu_ActivatesQuitPanel()
    {
        var exitScript = new GameObject().AddComponent<Exit>();
        var quitPanel = new GameObject();
        exitScript.quitPanel = quitPanel;

        exitScript.openConfirmationMenu();

        yield return null;
        Assert.IsTrue(quitPanel.activeSelf);
    }

    [UnityTest]
    public IEnumerator CloseConfirmationMenu_DeactivatesQuitPanel()
    {
        var exitScript = new GameObject().AddComponent<Exit>();
        var quitPanel = new GameObject();
        exitScript.quitPanel = quitPanel;
        exitScript.openConfirmationMenu();
        yield return null;

        exitScript.closeConfirmationMenu();

        Assert.IsFalse(quitPanel.activeSelf);
    }

    // Can only be tested when an actually application is built, cannot be tested via editor
    /*
    [Test]
    public void QuitGame_QuitsApplication()
    {
        var exitScript = new GameObject().AddComponent<Exit>();
        bool quitCalled = false;
        Application.quitting += () => quitCalled = true;

        exitScript.QuitGame();

        Assert.IsTrue(quitCalled);
    }
    */
}
