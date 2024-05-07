using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    private Camera _mainCamera;
    public GameObject ShopCanvas;

    public static Slot prevSlot;
    public static TowerMenu tower;
    public static TowerSelectionButton towerSelectionButton;

    private void Awake()
    {
        _mainCamera = Camera.main;
        
    }
    public void OnClick(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        var rayHit = Physics2D.GetRayIntersection(_mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue()));
        if (!rayHit.collider) return;
        
        if (prevSlot != null)
        {
            if(rayHit.collider.gameObject.name != prevSlot.gameObject.name)
            {
                if(prevSlot.SpriteRenderer.color == Color.green)
                {
                    prevSlot.ChangeColor();
                }
                prevSlot.Clicked = false;
                prevSlot = null;
            }
        }

        Debug.Log(rayHit.collider.gameObject.name);
        if (!rayHit.collider.gameObject.name.Contains("Slot"))
        {
            TowerSelectionButton.CurrentSlot = null;
        }
        if(tower != null)
        {
            if (rayHit.collider.gameObject.name != tower.gameObject.name)
            {
                tower.TowerMenuCanvas.SetActive(false);
            }
        }
    }
}
