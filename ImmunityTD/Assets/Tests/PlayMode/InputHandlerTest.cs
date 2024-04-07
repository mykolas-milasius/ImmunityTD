using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class InputHandlerTest
{
    private GameObject inputHandlerGameObject;
    private InputHandler inputHandlerScript;

    // Setup for each test
    [SetUp]
    public void Setup()
    {
        // Create a new GameObject for the InputHandler script and add the InputHandler component
        inputHandlerGameObject = new GameObject("InputHandler");
        inputHandlerScript = inputHandlerGameObject.AddComponent<InputHandler>();
    }

    // Test to verify behavior when no ray hit occurs
    [Test]
    public void OnClick_NoRayHit_Test()
    {
        // Call the OnClick method with a dummy InputAction.CallbackContext
        inputHandlerScript.OnClick(new InputAction.CallbackContext());

        // Assert that the prevSlot is null
        Assert.IsNull(InputHandler.prevSlot, "prevSlot should be null when no ray hit occurs");
    }

    // Cleanup after each test
    [TearDown]
    public void Teardown()
    {
        // Destroy the GameObject to clean up the environment for the next test
        Object.DestroyImmediate(inputHandlerGameObject);
    }
}
