using UnityEngine;

public class Slot : MonoBehaviour
{
    public GameObject towerSelectionCanvas; // Reference to the tower selection canvas

    private void OnMouseDown()
    {
        TowerSelectionButton.currentSlot = this;
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        // Activate tower selection canvas
        if (spriteRenderer.color == Color.green)
        {
            spriteRenderer.color = Color.white;
        }
        else if(spriteRenderer.color == Color.white)
        {
            spriteRenderer.color = Color.green;
        }
    }

    // Instantiate the selected tower prefab at the slot's position
    public void PlaceTower(GameObject towerPrefab)
    {
        // Instantiate the selected tower prefab at the slot's position
        if (towerPrefab != null)
        {
            Instantiate(towerPrefab, transform.position, Quaternion.identity);
        }
    }
}