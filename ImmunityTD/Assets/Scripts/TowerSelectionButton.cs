using Assets.Scripts;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts
{
    public class TowerSelectionButton : MonoBehaviour
    {
        public GameObject TowerPrefab;
        public static Slot CurrentSlot = null;
        public Canvas PurchaseMenu;
        public float Price;

        public void Start()
        {
            if (TowerPrefab == null)
            {
                Debug.LogError("Tower prefab is not assigned to the Tower selection Button!");
            }

            InputHandler.TowerSelectionButton = this;
        }

        public void OnTowerButtonClick()
        {
            PurchaseButton.CowerButton = this;
        }

        public void Place()
        {
            if (CurrentSlot != null)
            {
                if (CurrentSlot.Clicked)
                {
                    if (Player.Coins - Price >= 0)
                    {
                        Player.Coins -= Price;
                        CurrentSlot.PlaceTower(TowerPrefab);
                        TowerMenu.CurrentSlot = CurrentSlot;
                        CurrentSlot = null;
                    }
                    else
                    {
                        Debug.LogWarning("Not enough coins.");
                    }
                }
                else
                {
                    Debug.LogWarning("Slot is not clicked.");
                }
            }
            else
            {
                Debug.LogWarning("No slot selected.");
            }
        }
    }
}
