using UnityEngine;
using System;

public class HotbarManager : MonoBehaviour
{
    [SerializeField] int slotCount = 3;
    public GameObject[] slots;
    public int currentSelectedIndex { get; private set; } = 0;

    public event Action OnInventoryChanged;
    void Awake() => slots = new GameObject[slotCount];

    void Start() => SelectSlot(0);

    void Update()
    {
        for (int i = 0; i < slotCount; i++)if (Input.GetKeyDown(KeyCode.Alpha1 + i)) SelectSlot(i);
    }

    public void PickupItem(GameObject item)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null)
            {
                slots[i] = item;
                if (i == currentSelectedIndex) Bootstrapper.Inventory.TakeObject(item);
                else item.SetActive(false);
                OnInventoryChanged?.Invoke();
                return;
            }
        }
    }

    public void SelectSlot(int index)
    {
        if (index < 0 || index >= slotCount) return;
        if (index == currentSelectedIndex && Bootstrapper.Inventory.CurrentObject != null) return; 

        currentSelectedIndex = index;
        Bootstrapper.Inventory.HideObjectInPocket();

        GameObject selectedItem = slots[currentSelectedIndex];
        if (selectedItem != null) Bootstrapper.Inventory.TakeObject(selectedItem);
        OnInventoryChanged?.Invoke();
    }

    public void SwapItems(int indexA, int indexB)
    {
        GameObject temp = slots[indexA];
        slots[indexA] = slots[indexB];
        slots[indexB] = temp;
        SelectSlot(currentSelectedIndex);
        OnInventoryChanged?.Invoke();
    }

    public void RemoveCurrentItem()
    {
        slots[currentSelectedIndex] = null;
        OnInventoryChanged?.Invoke();
    }
}
