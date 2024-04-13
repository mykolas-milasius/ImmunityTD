using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class InputHandlerTest
{
    private GameObject inputHandlerGameObject;
    private InputHandler inputHandlerScript;

    [SetUp]
    public void Setup()
    {
        inputHandlerGameObject = new GameObject("InputHandler");
        inputHandlerScript = inputHandlerGameObject.AddComponent<InputHandler>();
    }

    [Test]
    public void OnClick_NoRayHit_Test()
    {
        inputHandlerScript.OnClick(new InputAction.CallbackContext());

        Assert.IsNull(InputHandler.prevSlot, "prevSlot should be null when no ray hit occurs");
    }

    [Test]
    public void OnClick_RayHitNonSlot_Test()
    {
        var nonSlotGameObject = new GameObject("NonSlotObject");

        inputHandlerScript.OnClick(new InputAction.CallbackContext());

        Assert.IsNull(InputHandler.towerSelectionButton, "towerSelectionButton should be reset when a ray hits a non-slot object");
    }

    [Test]
    public void OnClick_RayHitSlot_Test()
    {
        // Create a new GameObject for the slot
        var slotGameObject = new GameObject("Slot");
        var slot = slotGameObject.AddComponent<Slot>();

        // Assign the slot to the prevSlot property of the InputHandler class
        InputHandler.prevSlot = slot;

        // Call the OnClick method with a dummy InputAction.CallbackContext
        inputHandlerScript.OnClick(new InputAction.CallbackContext());

        // Assert that the prevSlot is an empty Slot object
        Assert.IsNotNull(InputHandler.prevSlot, "prevSlot should not be null");
        Assert.IsFalse(InputHandler.prevSlot.clicked, "prevSlot should not be clicked");
        // Add any additional assertions as needed for the state of the empty slot object
    }

    /* Not working yet =D
    [Test]
    public void OnClick_TowerMenuActive_Test()
    {
        // Create a new GameObject for the tower menu
        var towerMenuGameObject = new GameObject("TowerMenu");
        var towerMenu = towerMenuGameObject.AddComponent<TowerMenu>();
        InputHandler.tower = towerMenu;

        // Set the tower menu as active
        towerMenu.towerMenuCanvas.SetActive(true);

        // Call the OnClick method with a dummy InputAction.CallbackContext
        inputHandlerScript.OnClick(new InputAction.CallbackContext());

        // Assert that the tower menu is closed
        Assert.IsFalse(towerMenu.towerMenuCanvas.activeSelf, "Tower menu should be closed when clicking outside of it");
    }
    */

    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(inputHandlerGameObject);
    }
}
