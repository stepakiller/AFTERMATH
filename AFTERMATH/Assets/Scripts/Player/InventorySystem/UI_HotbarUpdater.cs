using UnityEngine;

public class UI_HotbarUpdater : MonoBehaviour
{
    [SerializeField] UI_Slot[] uiSlots;
    [SerializeField] UI_DraggableItem[] uiItems;

    void Start()
    {
        for (int i = 0; i < uiSlots.Length; i++)
        {
            uiSlots[i].slotIndex = i;
            uiItems[i].slotIndex = i;
            uiSlots[i].containerType = ItemContainer.Hotbar;
            uiItems[i].containerType = ItemContainer.Hotbar;
        }
        
        Bootstrapper.HotbarManager.OnInventoryChanged += UpdateUI;
        UpdateUI(); 
    }

    void OnDestroy()
    {
        if (Bootstrapper.HotbarManager != null) Bootstrapper.HotbarManager.OnInventoryChanged -= UpdateUI;
    }

    void UpdateUI()
    {
        ItemSettings[] items = Bootstrapper.HotbarManager.slots;
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] != null) uiItems[i].Setup(items[i].ItemData, i);
            else uiItems[i].Setup(null, i);
        }
    }
}
