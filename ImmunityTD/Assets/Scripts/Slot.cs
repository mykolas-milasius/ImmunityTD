using UnityEngine;

public class Slot : MonoBehaviour
{
    public GameObject towerSelectionCanvas; // Reference to the tower selection canvas
    public bool clicked = false;

    public void OnMouseDown()
    {
        TowerSelectionButton.currentSlot = this;
        PurchaseButton.currentSlot = this;
        InputHandler.prevSlot = this;
        clicked = !clicked;
        ChangeColor();
    }
    public void ChangeColor()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer.color == Color.green)
        {
            spriteRenderer.color = Color.white;
        }
        else if (spriteRenderer.color == Color.white)
        {
            spriteRenderer.color = Color.green;
        }
    }
    public void PlaceTower(GameObject towerPrefab)
    {
        // Instantiate the selected tower prefab at the slot's position
        if (towerPrefab != null)
        {
            Instantiate(towerPrefab, transform.position, Quaternion.identity);
            this.gameObject.SetActive(false);
        }
    }
}