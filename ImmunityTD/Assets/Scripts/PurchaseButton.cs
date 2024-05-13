using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class PurchaseButton : MonoBehaviour
    {
        public static Slot CurrentSlot; // Static reference to the current slot
        public static TowerSelectionButton TowerButton;
        public Button Button;

        public void Start()
        {
            Button.interactable = false;
        }

        public void Update()
        {
            if (TowerButton != null)
            {
                if (Player.Coins >= TowerButton.Price)
                {
                    Button.interactable = true;
                }
                else
                {
                    Button.interactable = false;
                }
            }
        }

        public void PurchaseOnClick()
        {
            // Ensure CurrentSlot is not null
            if (CurrentSlot != null)
            {
                if (TowerButton != null)
                {
                    TowerButton.Place();
                    TowerButton = null;
                }
                else
                {
                    Debug.LogWarning("No Tower selected.");
                }

            }
            else
            {
                Debug.LogWarning("No slot selected for Tower placement.");
            }
        }
    }
}