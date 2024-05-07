using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class TowerSelectionButtonTests
{

    private GameObject towerSelectionButtonObject;
    private TowerSelectionButton towerSelectionButton;

    [TearDown]
    public void TearDown()
    {
        // Clean up
        GameObject.DestroyImmediate(towerSelectionButton);
        GameObject.DestroyImmediate(towerSelectionButtonObject);
    }

    [Test]
    public void TowerPrefabNotAssigned_ErrorLogged()
    {
        // Arrange
        GameObject towerSelectionButtonObject = new GameObject();
        TowerSelectionButton towerSelectionButton = towerSelectionButtonObject.AddComponent<TowerSelectionButton>();

        // Act
        towerSelectionButton.Start();

        // Assert
        LogAssert.Expect(LogType.Error, "Tower prefab is not assigned to the Tower selection Button!");
    }
    [UnityTest]
    public IEnumerator PlaceTower_CurrentSlotNull_NoTowerPlaced()
    {
        // Arrange
        GameObject towerSelectionButtonObject = new GameObject();
        TowerSelectionButton towerSelectionButton = towerSelectionButtonObject.AddComponent<TowerSelectionButton>();
        TowerSelectionButton.CurrentSlot = null; // Set CurrentSlot to null
        towerSelectionButton.TowerPrefab = new GameObject();
        Player.Coins = 20; // Set coins to 20
        towerSelectionButton.Price = 10f; // Set price

        // Act
        towerSelectionButton.Place();
        yield return null; // Wait for a frame to process Unity's warning

        // Assert
        LogAssert.Expect(LogType.Warning, "No slot selected.");
        Assert.AreEqual(20, Player.Coins); // Coins remain unchanged
    }

    [UnityTest]
    public IEnumerator PlaceTower_NotEnoughCoins_NoTowerPlaced()
    {
        GameObject towerSelectionButtonObject = new GameObject();
        TowerSelectionButton towerSelectionButton = towerSelectionButtonObject.AddComponent<TowerSelectionButton>();
        towerSelectionButton.TowerPrefab = new GameObject();
        TowerSelectionButton.CurrentSlot = towerSelectionButtonObject.AddComponent<Slot>(); // Mock CurrentSlot

        TowerSelectionButton.CurrentSlot.Clicked = true;
        Player.Coins = 0; // Set coins to 0
        towerSelectionButton.Price = 10f; // Set price

        // Act
        towerSelectionButton.Place();
        yield return null; // Wait for a frame to process Unity's warning

        // Assert
        LogAssert.Expect(LogType.Warning, "Not enough coins.");
        Assert.IsNotNull(TowerSelectionButton.CurrentSlot); // Ensure no Tower is placed
    }

    [UnityTest]
    public IEnumerator PlaceTower_EnoughCoins_TowerPlaced()
    {
        GameObject towerSelectionButtonObject = new GameObject();
        TowerSelectionButton towerSelectionButton = towerSelectionButtonObject.AddComponent<TowerSelectionButton>();
        TowerSelectionButton.CurrentSlot = towerSelectionButtonObject.AddComponent<Slot>(); // Mock CurrentSlot
        towerSelectionButton.TowerPrefab = new GameObject();


        TowerSelectionButton.CurrentSlot.Clicked = true;
        Player.Coins = 20; // Set coins to 20
        towerSelectionButton.Price = 10f; // Set price

        // Act
        towerSelectionButton.Place();
        yield return null; // Wait for a frame to process Unity's warning

        // Assert
        Assert.AreEqual(10, Player.Coins); // Coins deducted correctly
        Assert.IsNull(TowerSelectionButton.CurrentSlot); // Tower placed
    }
    [UnityTest]
    public IEnumerator OnTowerButtonClick_PurchaseButtonThis()
    {
        GameObject towerSelectionButtonObject = new GameObject();
        TowerSelectionButton towerSelectionButton = towerSelectionButtonObject.AddComponent<TowerSelectionButton>();
        towerSelectionButton.TowerPrefab = new GameObject();

        towerSelectionButton.OnTowerButtonClick();
        yield return null; // Wait for a frame to process Unity's warning

        // Assert
        Assert.AreEqual(towerSelectionButton, PurchaseButton.towerButton);
    }
    [UnityTest]
    public IEnumerator Place_Slot_IsClicked()
    {
        GameObject towerSelectionButtonObject = new GameObject();
        TowerSelectionButton towerSelectionButton = towerSelectionButtonObject.AddComponent<TowerSelectionButton>();
        towerSelectionButton.TowerPrefab = new GameObject();

        TowerSelectionButton.CurrentSlot = towerSelectionButtonObject.AddComponent<Slot>(); // Mock CurrentSlot


        towerSelectionButton.Place();
        yield return null; // Wait for a frame to process Unity's warning

        LogAssert.Expect(LogType.Warning, "Slot is not clicked.");
    }
}
