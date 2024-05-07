using UnityEngine;

namespace Assets.Scripts
{
    public class TowerMenu : MonoBehaviour
    {
        public GameObject TowerMenuCanvas;
        public GameObject TowerRangePreview;
        public SpriteRenderer RangeRenderer;
        public Canvas PurchaseMenu;
        public static Slot CurrentSlot;

        private bool _state = false;

        public void OnMouseDown()
        {
            InputHandler.tower = this;
            // Check if the tower menu canvas is assigned
            if (TowerMenuCanvas != null)
            {
                if (!_state)
                {
                    TowerMenuCanvas.SetActive(true);
                    RangeRenderer.enabled = true;
                    _state = true;
                }
                else if (_state)
                {
                    TowerMenuCanvas.SetActive(false);
                    RangeRenderer.enabled = false;
                    _state = false;
                }
            }
            else
            {
                Debug.LogError("Tower menu canvas is not assigned to the tower prefab!");
            }
        }

        public void RemoveTower()
        {
            if (CurrentSlot != null)
            {
                CurrentSlot.gameObject.SetActive(true);
            }

            Destroy(gameObject);
        }
    }
}