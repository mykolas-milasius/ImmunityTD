using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    private Camera _mainCamera;
    public GameObject ShopCanvas;
    private bool state = false;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }
    public void OnClick(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        var rayHit = Physics2D.GetRayIntersection(_mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue()));
        if (!rayHit.collider) return;
        
        Debug.Log(rayHit.collider.gameObject.name);
        if (!rayHit.collider.gameObject.name.Contains("Slot"))
        {
            ShopCanvas.SetActive(false);
        }
        else
        {
            if (state)
            {
                state = false;
            }
            else
            {
                state = true;
            }
            ShopCanvas.SetActive(state);
        }
    }
}
