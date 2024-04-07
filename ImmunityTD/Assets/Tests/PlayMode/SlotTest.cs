using NUnit.Framework;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

public class SlotTest
{
    private GameObject slotGameObject;
    private Slot slotScript;
    private SpriteRenderer spriteRenderer;

    [SetUp]
    public void Setup()
    {
        // Create a new game object with Slot script attached
        slotGameObject = new GameObject();
        slotScript = slotGameObject.AddComponent<Slot>();
        spriteRenderer = slotGameObject.AddComponent<SpriteRenderer>();
    }

    [TearDown]
    public void TearDown()
    {
        // Clean up
        GameObject.Destroy(slotGameObject);
    }

    [UnityTest]
    public IEnumerator OnMouseDown_Test()
    {
        slotScript.towerSelectionCanvas = new GameObject(); // Create a mock tower selection canvas
        slotGameObject.AddComponent<InputHandler>(); // Add InputHandler component

        slotScript.OnMouseDown(); // Call the method

        yield return null; // Wait for one frame

        // Assert that TowerSelectionButton.currentSlot is set to this slot
        Assert.AreEqual(slotScript, TowerSelectionButton.currentSlot);
        // Assert that InputHandler.prevSlot is set to this slot
        Assert.AreEqual(slotScript, InputHandler.prevSlot);
    }

    [UnityTest]
    public IEnumerator ChangeColor_Test()
    {
        // Set the initial color to white
        spriteRenderer.color = Color.white;

        slotScript.ChangeColor(); // Call the method

        yield return null; // Wait for one frame

        // Assert that the color has changed to green
        Assert.AreEqual(Color.green, spriteRenderer.color);

        slotScript.ChangeColor(); // Call the method again

        yield return null; // Wait for one frame

        // Assert that the color has changed back to white
        Assert.AreEqual(Color.white, spriteRenderer.color);
    }
}