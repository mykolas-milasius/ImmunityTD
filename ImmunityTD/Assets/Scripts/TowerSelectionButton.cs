using UnityEngine;
using UnityEngine.Events;

public class TowerSelectionButton : MonoBehaviour
{
    public GameObject towerPrefab; // Reference to the tower prefab to be placed
    public static Slot currentSlot; // Static reference to the current slot
    private void Start()
    {
        // Ensure a tower prefab is assigned to the button
        if (towerPrefab == null)
        {
            Debug.LogError("Tower prefab is not assigned to the tower selection button!");
        }
    }
    public void OnTowerButtonClick()
    {
        PurchaseButton.towerButton = this;
    }
    public void Place()
    {
        currentSlot.PlaceTower(towerPrefab);
        TowerMenu.currentSlot = currentSlot;
        currentSlot = null;
    }
}
