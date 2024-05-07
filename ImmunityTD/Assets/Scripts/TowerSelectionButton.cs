using UnityEngine;
using UnityEngine.Events;

public class TowerSelectionButton : MonoBehaviour
{
    public GameObject towerPrefab;
    public static Slot currentSlot = null;
    public Canvas purchaseMenu;
    public float price;
    public void Start()
    {
        if (towerPrefab == null)
        {
            Debug.LogError("Tower prefab is not assigned to the tower selection button!");
        }
        InputHandler.towerSelectionButton = this;
    }
    public void OnTowerButtonClick()
    {
        PurchaseButton.towerButton = this;
    }
    public void Place()
    {
        if (currentSlot != null)
        {
            if (currentSlot.clicked)
            {
                if (Player.Coins - price >= 0)
                {
                    Player.Coins -= price;
                    currentSlot.PlaceTower(towerPrefab);
                    TowerMenu.currentSlot = currentSlot;
                    currentSlot = null;
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
