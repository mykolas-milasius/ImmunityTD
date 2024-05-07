using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PurchaseButton : MonoBehaviour
{
    public static Slot currentSlot; // Static reference to the current slot
    public static TowerSelectionButton towerButton;
    public Button button;

    public void Start()
    {
        button.interactable = false;
    }

    public void Update()
    {
        if (towerButton != null)
        {
            if (Player.Coins >= towerButton.Price)
            {
                button.interactable = true;
            }
            else
            {
                button.interactable = false;
            }
        }
    }
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