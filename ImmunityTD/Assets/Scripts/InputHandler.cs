using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    private Camera _mainCamera;
    public GameObject ShopCanvas;
    private GameObject previousClick;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }
    public void OnClick(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        var rayHit = Physics2D.GetRayIntersection(_mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue()));
        if (!rayHit.collider) return;
        Slot click;
        
        
        Debug.Log(rayHit.collider.gameObject.name);
        if (rayHit.collider.gameObject.name.Contains("Slot"))
        {
            //rayHit.collider.gameObject.GetComponent<SpriteRenderer>().color = Color.green;
        }
    }
}
