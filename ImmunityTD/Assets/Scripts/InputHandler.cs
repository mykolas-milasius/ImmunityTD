using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts
{
    public class InputHandler : MonoBehaviour
    {
        private Camera _mainCamera;

        public static Slot PrevSlot;
        public static TowerMenu Tower;
        public static TowerSelectionButton TowerSelectionButton;

        private void Awake()
        {
            _mainCamera = Camera.main;

        }

        public void OnClick(InputAction.CallbackContext context)
        {
            if (!context.started) return;

            var rayHit = Physics2D.GetRayIntersection(_mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue()));
            if (!rayHit.collider) return;

            if (PrevSlot != null)
            {
                if (rayHit.collider.gameObject.name != PrevSlot.gameObject.name)
                {
                    if (PrevSlot.SpriteRenderer.color == Color.green)
                    {
                        PrevSlot.ChangeColor();
                    }

                    PrevSlot.Clicked = false;
                    PrevSlot = null;
                }
            }

            Debug.Log(rayHit.collider.gameObject.name);
            if (!rayHit.collider.gameObject.name.Contains("Slot"))
            {
                TowerSelectionButton.CurrentSlot = null;
            }

            if (Tower != null)
            {
                if (rayHit.collider.gameObject.name != Tower.gameObject.name)
                {
                    Tower.TowerMenuCanvas.SetActive(false);
                }
            }
        }
    }
}
