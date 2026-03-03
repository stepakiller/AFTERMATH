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
        GameObject[] itemObjects = Bootstrapper.HotbarManager.slots;
        for (int i = 0; i < itemObjects.Length; i++)
        {
            if (itemObjects[i] != null)
            {
                ItemSettings settings = itemObjects[i].GetComponent<ItemSettings>();
                uiItems[i].Setup(settings.ItemData, i);
            }
            else uiItems[i].Setup(null, i);
        }
    }
}
