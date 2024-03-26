using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurchaseButton : MonoBehaviour
{
    public static Slot currentSlot; // Static reference to the current slot
    public static TowerSelectionButton towerButton;
    public void PurchaseOnClick()
    {
        // Ensure currentSlot is not null
        if (currentSlot != null)
        {
            if (towerButton != null)
            {
                towerButton.Place();
                towerButton = null;
            }
            else
            {
                Debug.LogWarning("No tower selected.");
            }
            
        }
        else
        {
            Debug.LogWarning("No slot selected for tower placement.");
        }
    }
}