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

    // Method to be called when the tower selection button is clicked
    public void OnTowerButtonClick()
    {
        // Ensure currentSlot is not null
        if (currentSlot != null)
        {
            // Place tower at the current slot
            currentSlot.PlaceTower(towerPrefab);
            currentSlot = null;
        }
        else
        {
            Debug.LogWarning("No slot selected for tower placement.");
        }
    }
}
