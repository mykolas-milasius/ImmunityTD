using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerMenu : MonoBehaviour
{
    public GameObject towerMenuCanvas; // Reference to the tower menu canvas
    public static Slot currentSlot;
    private bool state = false;

    private void OnMouseDown()
    {
        InputHandler.tower = this;
        // Check if the tower menu canvas is assigned
        if (towerMenuCanvas != null)
        {
            if (!state)
            {
                towerMenuCanvas.SetActive(true);
                state = true;
            }
            else if (state)
            {
                towerMenuCanvas.SetActive(false);
                state = false;
            }
        }
        else
        {
            Debug.LogError("Tower menu canvas is not assigned to the tower prefab!");
        }
    }
    public void RemoveTower()
    {
        Destroy(gameObject);
        currentSlot.gameObject.SetActive(true);
    }
}
