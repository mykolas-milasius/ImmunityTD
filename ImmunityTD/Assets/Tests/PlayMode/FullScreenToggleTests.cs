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
        var mockScreenService = new MockScreenService
        {
            IsFullScreen = false // Initial state
        };

        fullScreenToggle.ScreenService = mockScreenService;

        fullScreenToggle.ToggleFullScreen();

        Assert.IsTrue(mockScreenService.IsFullScreen, "FullScreen state should be toggled to true.");

        fullScreenToggle.ToggleFullScreen();

        Assert.IsFalse(mockScreenService.IsFullScreen, "FullScreen state should be toggled back to false.");
    }
}
public class MockScreenService : IScreenService
{
    public bool IsFullScreen { get; set; }
}

public class ScreenServiceTests
{
    private ScreenService screenService;

    [SetUp]
    public void SetUp()
    {
        screenService = new ScreenService();
    }

    [Test]
    public void SetIsFullScreen_UpdatesScreenFullScreenState()
    {
        // Initially set to the opposite of the current state to ensure the test is valid
        screenService.IsFullScreen = !Screen.fullScreen;

        // Assert that setting IsFullScreen updates Screen.fullScreen
        Assert.AreEqual(screenService.IsFullScreen, Screen.fullScreen);

        // Optionally, reset the state to avoid changing the development environment
        screenService.IsFullScreen = !screenService.IsFullScreen;
    }

    [Test]
    public void GetIsFullScreen_ReturnsCurrentScreenFullScreenState()
    {
        // Skip this test if running in an environment that doesn't support full-screen changes
        if (Application.isBatchMode || Application.isEditor)
        {
            Assert.Inconclusive("Skipping full-screen test in the current environment.");
            return;
        }

        // Proceed with the test if the environment supports full-screen changes
        Screen.fullScreen = true;
        Assert.IsTrue(screenService.IsFullScreen);

        Screen.fullScreen = false;
        Assert.IsFalse(screenService.IsFullScreen);
    }
}