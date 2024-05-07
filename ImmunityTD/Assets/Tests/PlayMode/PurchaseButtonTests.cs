using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

public class PurchaseButtonTests
{
    [UnityTest]
    public IEnumerator PurchaseButton_InteractableWhenEnoughCoins()
    {
        // Arrange
        GameObject purchaseButtonObject = new GameObject();
        PurchaseButton purchaseButton = purchaseButtonObject.AddComponent<PurchaseButton>();
        Button buttonComponent = purchaseButtonObject.AddComponent<Button>();
        purchaseButton.button = buttonComponent;
        GameObject towerSelectionButtonObject = new GameObject();
        TowerSelectionButton towerSelectionButton = towerSelectionButtonObject.AddComponent<TowerSelectionButton>();
        towerSelectionButton.Price = 50f; // Set Tower price
        towerSelectionButton.TowerPrefab = towerSelectionButtonObject;

        PurchaseButton.towerButton = towerSelectionButton;
        Player.Coins = 100; // Set player coins to a value more than Tower price

        // Act
        purchaseButton.Start(); // Call Start to initialize Button state
        purchaseButton.Update(); // Call Update to check coin amount
        yield return null; // Wait for a frame to process Tower placement

        // Assert
        Assert.IsTrue(purchaseButton.button.interactable); // Button should be interactable when player has enough coins
    }
    [UnityTest]
    public IEnumerator PurchaseButton_SlotIsNull()
    {
        // Arrange
        GameObject purchaseButtonObject = new GameObject();
        PurchaseButton purchaseButton = purchaseButtonObject.AddComponent<PurchaseButton>();
        Button buttonComponent = purchaseButtonObject.AddComponent<Button>();
        purchaseButton.button = buttonComponent;

        GameObject towerSelectionButtonObject = new GameObject();
        TowerSelectionButton towerSelectionButton = towerSelectionButtonObject.AddComponent<TowerSelectionButton>();
        towerSelectionButton.Price = 50f; // Set Tower price
        towerSelectionButton.TowerPrefab = towerSelectionButtonObject;

        PurchaseButton.currentSlot = null; // Mock current slot

        purchaseButton.PurchaseOnClick();
        yield return null; // Wait for a frame to process Tower placement

        LogAssert.Expect(LogType.Warning, "No slot selected for Tower placement.");
    }
    [UnityTest]
    public IEnumerator PurchaseButton_TowerButtonIsNull()
    {
        // Arrange
        GameObject purchaseButtonObject = new GameObject();
        PurchaseButton purchaseButton = purchaseButtonObject.AddComponent<PurchaseButton>();
        Button buttonComponent = purchaseButtonObject.AddComponent<Button>();
        purchaseButton.button = buttonComponent;

        GameObject towerSelectionButtonObject = new GameObject();
        TowerSelectionButton towerSelectionButton = towerSelectionButtonObject.AddComponent<TowerSelectionButton>();
        towerSelectionButton.Price = 50f; // Set Tower price
        towerSelectionButton.TowerPrefab = towerSelectionButtonObject;

        GameObject slotObject = new GameObject();
        Slot slot = slotObject.AddComponent<Slot>();
        PurchaseButton.currentSlot = slot; // Mock current slot
        PurchaseButton.towerButton = null;

        purchaseButton.PurchaseOnClick();
        yield return null; // Wait for a frame to process Tower placement

        LogAssert.Expect(LogType.Warning, "No Tower selected.");
    }
    [UnityTest]
    public IEnumerator PurchaseButton_PlaceTower()
    {
        // Arrange
        GameObject purchaseButtonObject = new GameObject();
        PurchaseButton purchaseButton = purchaseButtonObject.AddComponent<PurchaseButton>();
        Button buttonComponent = purchaseButtonObject.AddComponent<Button>();
        purchaseButton.button = buttonComponent;

        GameObject towerSelectionButtonObject = new GameObject();
        TowerSelectionButton towerSelectionButton = towerSelectionButtonObject.AddComponent<TowerSelectionButton>();
        towerSelectionButton.Price = 50f; // Set Tower price
        towerSelectionButton.TowerPrefab = towerSelectionButtonObject;
        Player.Coins = 60;

        GameObject slotObject = new GameObject();
        Slot slot = slotObject.AddComponent<Slot>();
        PurchaseButton.currentSlot = slot; // Mock current slot
        PurchaseButton.currentSlot.Clicked = true;
        TowerSelectionButton.CurrentSlot = PurchaseButton.currentSlot;

        PurchaseButton.towerButton = towerSelectionButton;

        purchaseButton.PurchaseOnClick();
        yield return null; // Wait for a frame to process Tower placement

        Assert.IsNull(TowerSelectionButton.CurrentSlot);
    }
}
