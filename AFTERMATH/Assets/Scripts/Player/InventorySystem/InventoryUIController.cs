using UnityEngine;

public class InventoryUIController : MonoBehaviour
{
    public GameObject inventoryCanvas;
    bool isInventoryOpen = false;

    void Start()
    {
        inventoryCanvas.SetActive(false);
        InputManager.Instance.OnInventoryPressed += ToggleInventory;
    }

    void OnDestroy()
    {
        if (InputManager.Instance != null) InputManager.Instance.OnInventoryPressed -= ToggleInventory;
    }

    void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen;
        inventoryCanvas.SetActive(isInventoryOpen);

        if (isInventoryOpen)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            InputManager.Instance.EnableUIInput();
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            InputManager.Instance.EnablePlayerInput();
        }
    }
}
