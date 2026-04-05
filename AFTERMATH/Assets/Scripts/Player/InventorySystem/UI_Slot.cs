using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Slot : MonoBehaviour, IDropHandler
{
    public int slotIndex;
    public ItemContainer containerType; // Указываем в инспекторе (Hotbar или Radial)
    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedObj = eventData.pointerDrag;
        if (droppedObj == null) return;

        UI_DraggableItem draggableItem = droppedObj.GetComponent<UI_DraggableItem>();
        if (draggableItem != null) 
        {
            // Вызываем новый универсальный метод перемещения
            Bootstrapper.HotbarManager.MoveItem(
                draggableItem.containerType, draggableItem.slotIndex, 
                this.containerType, this.slotIndex);
        }
    }
}
