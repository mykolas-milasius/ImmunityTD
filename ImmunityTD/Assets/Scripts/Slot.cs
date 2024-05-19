using UnityEngine;

namespace Assets.Scripts
{
    public class Slot : MonoBehaviour
    {
        public GameObject ShopUICanvas;
        public bool Clicked = false;
        public SpriteRenderer SpriteRenderer;
        public TowerSelectionButton TowerSelectionButton;

        public void Start()
        {
            // Ensure spriteRenderer is assigned
            if (SpriteRenderer == null)
            {
                SpriteRenderer = GetComponent<SpriteRenderer>();
            }
        }


        public void OnMouseDown()
        {
            TowerSelectionButton.CurrentSlot = this;
            PurchaseButton.CurrentSlot = this;
            InputHandler.PrevSlot = this;
            Clicked = !Clicked;
            ChangeColor();
            //ShopUICanvas.SetActive(true);
        }

        public void ChangeColor()
        {

            if (SpriteRenderer.color == Color.green)
            {
                SpriteRenderer.color = Color.white;
            }
            else if (SpriteRenderer.color == Color.white)
            {
                SpriteRenderer.color = Color.green;
            }
        }

        public void PlaceTower(GameObject towerPrefab)
        {
            // Instantiate the selected Tower prefab at the slot's position
            if (towerPrefab != null)
            {
                Instantiate(towerPrefab, transform.position, Quaternion.identity);
                this.gameObject.SetActive(false);
            }
        }
    }
}