using NUnit.Framework;
using UnityEngine;

public class FullScreenToggleTests
{
    private GameObject gameObject;
    private FullScreenToggle fullScreenToggle;

    [SetUp]
    public void SetUp()
    {
        // Set up the GameObject with the FullScreenToggle component
        gameObject = new GameObject("FullScreenToggleObject");
        fullScreenToggle = gameObject.AddComponent<FullScreenToggle>();
    }

    [TearDown]
    public void TearDown()
    {
        // Clean up
        GameObject.DestroyImmediate(gameObject);
    }

    [Test]
    public void ToggleFullScreen_TogglesScreenState()
    {
        // Capture the initial full screen state
        bool initialState = Screen.fullScreen;

        // Call the ToggleFullScreen method
        fullScreenToggle.ToggleFullScreen();

        // Check if the full screen state has been toggled
        Assert.AreNotEqual(initialState, Screen.fullScreen, "FullScreen state should be toggled.");

        // Optionally, you can toggle back to ensure it's reversible, but it might affect your test environment
        fullScreenToggle.ToggleFullScreen();
        Assert.AreEqual(initialState, Screen.fullScreen, "Toggling full screen twice should revert to the original state.");
    }
}
