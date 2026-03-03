using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Slot : MonoBehaviour, IDropHandler
{
    public int slotIndex;

    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedObj = eventData.pointerDrag;
        if (droppedObj == null) return;

        UI_DraggableItem draggableItem = droppedObj.GetComponent<UI_DraggableItem>();
        if (draggableItem != null) Bootstrapper.HotbarManager.SwapItems(draggableItem.slotIndex, this.slotIndex);
    }
}
