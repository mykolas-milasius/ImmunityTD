using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor.SceneManagement;
using System.Collections;

public class SlotTest
{
    private GameObject slotGameObject;
    private Slot slotScript;
    private SpriteRenderer spriteRenderer;
    private GameObject towerPrefab;

    [SetUp]
    public void Setup()
    {
        // Create a new game object with Slot script attached
        slotGameObject = new GameObject();
        slotScript = slotGameObject.AddComponent<Slot>();
        spriteRenderer = slotGameObject.AddComponent<SpriteRenderer>();
        slotScript.spriteRenderer = spriteRenderer; // Ensure spriteRenderer is assigned

        slotScript.towerSelectionCanvas = new GameObject(); // Create a mock tower selection canvas
        slotScript.towerSelectionCanvas.SetActive(false); // Initially set it to inactive

        towerPrefab = new GameObject(); // Mock tower prefab
    }

    [TearDown]
    public void TearDown()
    {
        // Clean up
        GameObject.DestroyImmediate(slotGameObject);
        GameObject.DestroyImmediate(towerPrefab);
    }

    [Test]
    public void Start_AssignsSpriteRenderer_IfNotSet()
    {
        slotScript.spriteRenderer = null; // Simulate unassigned spriteRenderer

        slotScript.Start(); // Call Start manually as Unity does not call it in Edit Mode

        // Assert that spriteRenderer is now assigned
        Assert.IsNotNull(slotScript.spriteRenderer);
    }

    [UnityTest]
    public IEnumerator OnMouseDown_TogglesClickedAndActivatesCanvas()
    {
        slotScript.OnMouseDown(); // Simulate mouse down event

        yield return null; // Wait for one frame

        // Assert that 'clicked' is toggled
        Assert.IsTrue(slotScript.clicked);
        // Assert that towerSelectionCanvas is active
        Assert.IsTrue(slotScript.towerSelectionCanvas.activeSelf);
        // Assert that the color changes to green initially (assuming starting color is not green)
        Assert.AreEqual(Color.green, spriteRenderer.color);

        // Simulate another mouse down to toggle back
        slotScript.OnMouseDown();

        yield return null; // Wait for one frame

        // Assert that 'clicked' is toggled back
        Assert.IsFalse(slotScript.clicked);
        // Color should toggle back to white
        Assert.AreEqual(Color.white, spriteRenderer.color);
    }

    [UnityTest]
    public IEnumerator PlaceTower_InstantiatesTowerAndDeactivatesSlot()
    {
        slotScript.PlaceTower(towerPrefab); // Call PlaceTower with the mock prefab

        yield return null; // Wait for one frame to allow Instantiate to complete

        // Assert that a tower instance was created at the slot's position
        var towerInstance = GameObject.Find(towerPrefab.name + "(Clone)");
        Assert.IsNotNull(towerInstance);
        Assert.AreEqual(slotGameObject.transform.position, towerInstance.transform.position);

        // Assert that the slot game object is deactivated
        Assert.IsFalse(slotGameObject.activeSelf);

        GameObject.DestroyImmediate(towerInstance); // Clean up the instantiated tower
    }
}
