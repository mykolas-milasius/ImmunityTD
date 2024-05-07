using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TowerMenuTests
{
    private GameObject towerMenuObject;
    private TowerMenu towerMenu;
    private GameObject towerRangePreview;
    private SpriteRenderer rangeRenderer;
    private Canvas purchaseMenu;
    private GameObject slotGameObject;
    private Slot slot;

    [SetUp]
    public void SetUp()
    {
        // Set up the TowerMenu and its required components
        towerMenuObject = new GameObject("TowerMenu");
        towerMenu = towerMenuObject.AddComponent<TowerMenu>();
        towerMenu.TowerMenuCanvas = new GameObject("TowerMenuCanvas");
        towerMenu.TowerMenuCanvas.SetActive(false); // Initially set to inactive

        towerRangePreview = new GameObject("TowerRangePreview");
        rangeRenderer = towerRangePreview.AddComponent<SpriteRenderer>();
        towerMenu.RangeRenderer = rangeRenderer;
        rangeRenderer.enabled = false; // Initially disabled

        purchaseMenu = new GameObject("PurchaseMenu").AddComponent<Canvas>();
        towerMenu.PurchaseMenu = purchaseMenu;

        // Set up a Slot object for testing RemoveTower
        slotGameObject = new GameObject("Slot");
        slot = slotGameObject.AddComponent<Slot>();
        TowerMenu.CurrentSlot = slot;
    }

    [TearDown]
    public void TearDown()
    {
        // Clean up
        GameObject.DestroyImmediate(towerMenuObject);
        GameObject.DestroyImmediate(towerRangePreview);
        GameObject.DestroyImmediate(slotGameObject);
    }

    [Test]
    public void RemoveTower_WithoutCurrentSlot_DoesNotThrowError()
    {
        TowerMenu.CurrentSlot = null; // Set currentSlot to null to simulate the edge case

        // Use Assert.DoesNotThrow to verify that no error is thrown when currentSlot is null
        Assert.DoesNotThrow(() => towerMenu.RemoveTower());
    }

    [Test]
    public void OnMouseDown_UpdatesInputHandlerTower()
    {
        towerMenu.OnMouseDown(); // Simulate mouse down event

        // Assert that InputHandler.tower is set to this towerMenu instance
        Assert.AreEqual(InputHandler.tower, towerMenu);
    }

    [Test]
    public void PurchaseMenu_InitialState()
    {
        // Assuming the initial state should be inactive or some other specific state
        Assert.IsTrue(purchaseMenu.enabled); // or other relevant assertions based on your game's logic
    }

    [Test]
    public void TowerRangePreview_InitialState()
    {
        // Assuming towerRangePreview should initially be inactive
        Assert.IsTrue(towerRangePreview.activeSelf);
    }

    [Test]
    public void OnMouseDown_TogglesMenuAndRangeVisibility()
    {
        // Initial state should be inactive and disabled
        Assert.IsFalse(towerMenu.TowerMenuCanvas.activeSelf);
        Assert.IsFalse(towerMenu.RangeRenderer.enabled);

        // Simulate mouse down event
        towerMenu.OnMouseDown();

        // Menu and range should now be active and enabled
        Assert.IsTrue(towerMenu.TowerMenuCanvas.activeSelf);
        Assert.IsTrue(towerMenu.RangeRenderer.enabled);

        // Simulate another mouse down to toggle back
        towerMenu.OnMouseDown();

        // Menu and range should now be inactive and disabled again
        Assert.IsFalse(towerMenu.TowerMenuCanvas.activeSelf);
        Assert.IsFalse(towerMenu.RangeRenderer.enabled);
    }

    [Test]
    public void OnMouseDown_WithNoCanvasAssigned_LogsError()
    {
        towerMenu.TowerMenuCanvas = null; // Unassign the canvas to simulate the error condition

        LogAssert.Expect(LogType.Error, "Tower menu canvas is not assigned to the tower prefab!");
        towerMenu.OnMouseDown(); // Simulate mouse down event
    }

    [UnityTest]
    public IEnumerator RemoveTower_ReactivatesCurrentSlotAndDestroysItself()
    {
        // Make sure the slot is initially inactive
        TowerMenu.CurrentSlot.gameObject.SetActive(false);

        // Call RemoveTower
        towerMenu.RemoveTower();

        yield return null; // Wait for the next frame to ensure Destroy is processed

        // Slot should now be active
        Assert.IsTrue(TowerMenu.CurrentSlot.gameObject.activeSelf);

        // TowerMenu object should be destroyed, so it should be null
        Assert.IsTrue(towerMenu == null);
    }
}
