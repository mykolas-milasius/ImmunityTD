using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerMenu : MonoBehaviour
{
    public GameObject towerMenuCanvas;
    public GameObject towerRangePreview;
    public SpriteRenderer rangeRenderer;
    public Canvas purchaseMenu;
    public static Slot currentSlot;
    private bool state = false;


    public void OnMouseDown()
    {
        InputHandler.tower = this;
        // Check if the tower menu canvas is assigned
        if (towerMenuCanvas != null)
        {
            if (!state)
            {
                towerMenuCanvas.SetActive(true);
                rangeRenderer.enabled = true;
                state = true;
            }
            else if (state)
            {
                towerMenuCanvas.SetActive(false);
                rangeRenderer.enabled = false;
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
