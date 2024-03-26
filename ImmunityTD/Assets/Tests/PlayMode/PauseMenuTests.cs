using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PauseMenuTests
{
    private GameObject gameObject;
    private PauseMenu pauseMenu;
    private GameObject PauseMenuMock;
    private GameObject pauseButtonMock;

    [SetUp]
    public void Setup()
    {
        gameObject = new GameObject();
        pauseMenu = gameObject.AddComponent<PauseMenu>();

        PauseMenuMock = new GameObject("PauseMenu");
        pauseButtonMock = new GameObject("PauseButton");

        pauseMenu.pauseMenu = PauseMenuMock;
        pauseMenu.pauseButton = pauseButtonMock;

        PauseMenuMock.SetActive(false);
        pauseButtonMock.SetActive(true);
    }

    [Test]
    public void EnablePauseMenu_ActivatesPauseMenu_DeactivatesPauseButton()
    {
        pauseMenu.EnablePauseMenu();

        Assert.IsTrue(pauseMenu.pauseMenu.activeSelf);
        Assert.IsFalse(pauseMenu.pauseButton.activeSelf);
        Assert.IsTrue(pauseMenu.isPaused);
    }

    [Test]
    public void DisablePauseMenu_DeactivatesPauseMenu_ActivatesPauseButton()
    {
        pauseMenu.EnablePauseMenu();

        pauseMenu.DisablePauseMenu();

        Assert.IsFalse(pauseMenu.pauseMenu.activeSelf);
        Assert.IsTrue(pauseMenu.pauseButton.activeSelf);
        Assert.IsFalse(pauseMenu.isPaused);
    }


    [Test]
    public void EnablePauseMenu_ActivatesPauseMenuAndDeactivatesPauseButton()
    {
        pauseMenu.EnablePauseMenu();

        Assert.IsTrue(PauseMenuMock.activeSelf);
        Assert.IsFalse(pauseButtonMock.activeSelf);
        Assert.IsTrue(pauseMenu.isPaused);
    }

    [Test]
    public void DisablePauseMenu_DeactivatesPauseMenuAndActivatesPauseButton()
    {
        pauseMenu.EnablePauseMenu();

        pauseMenu.DisablePauseMenu();

        Assert.IsFalse(PauseMenuMock.activeSelf);
        Assert.IsTrue(pauseButtonMock.activeSelf);
        Assert.IsFalse(pauseMenu.isPaused);
    }

    [Test]
    public void PauseMenu_StartsWithCorrectInitialState()
    {
        Assert.IsFalse(PauseMenuMock.activeSelf, "Pause menu should initially be inactive.");
        Assert.IsTrue(pauseButtonMock.activeSelf, "Pause button should initially be active.");
    }

    [Test]
    public void EnablePauseMenu_MultipleCalls_SameEffect()
    {
        pauseMenu.EnablePauseMenu();
        pauseMenu.EnablePauseMenu();

        Assert.IsTrue(PauseMenuMock.activeSelf, "Pause menu should be active after enabling.");
        Assert.IsFalse(pauseButtonMock.activeSelf, "Pause button should be inactive after enabling.");
    }

    [Test]
    public void DisablePauseMenu_MultipleCalls_SameEffect()
    {
        pauseMenu.EnablePauseMenu();
        pauseMenu.DisablePauseMenu();
        pauseMenu.DisablePauseMenu();

        Assert.IsFalse(PauseMenuMock.activeSelf, "Pause menu should be inactive after disabling.");
        Assert.IsTrue(pauseButtonMock.activeSelf, "Pause button should be active after disabling.");
    }

    [TearDown]
    public void Teardown()
    {
        if (gameObject != null) Object.DestroyImmediate(gameObject);
        if (PauseMenuMock != null) Object.DestroyImmediate (PauseMenuMock);
        if (pauseButtonMock != null) Object.DestroyImmediate(pauseButtonMock);
    }
}