using UnityEngine;

public class Slot : MonoBehaviour
{
    public GameObject towerSelectionCanvas; // Reference to the tower selection canvas
    private GameObject selectedTowerPrefab; // The selected tower prefab

    private void OnMouseDown()
    {
        // Activate tower selection canvas
        towerSelectionCanvas.SetActive(true);
    }

    // Method to be called when a tower button is clicked
    public void SelectTower(GameObject towerPrefab)
    {
        selectedTowerPrefab = towerPrefab;
        PlaceTower();
    }

    // Instantiate the selected tower prefab at the slot's position
    private void PlaceTower()
    {
        if (selectedTowerPrefab != null)
        {
            Instantiate(selectedTowerPrefab, transform.position, Quaternion.identity);
            towerSelectionCanvas.SetActive(false); // Deactivate tower selection canvas after placing tower
        }
    }
}