using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public int slotIndex;
    Transform originalParent;
    Image image;
    CanvasGroup canvasGroup;
    Vector2 dragOffset;
    Canvas mainCanvas; 

    void Awake()
    {
        image = GetComponent<Image>();
        canvasGroup = GetComponent<CanvasGroup>();
        mainCanvas = GetComponentInParent<Canvas>();
        RectTransform rt = GetComponent<RectTransform>();
        rt.pivot = new Vector2(0.5f, 0.5f);
    }

    public void Setup(ItemData item, int index)
    {
        slotIndex = index;
        if (item != null)
        {
            image.sprite = item.icon;
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
        }
        else
        {
            image.sprite = null;
            canvasGroup.alpha = 0f;
            canvasGroup.blocksRaycasts = false;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        dragOffset = (Vector2)transform.position - eventData.position;
        originalParent = transform.parent;
        transform.SetParent(mainCanvas.transform, true); 
        transform.SetAsLastSibling();
        canvasGroup.blocksRaycasts = false; 
    }

    public void OnDrag(PointerEventData eventData) => transform.position = eventData.position + dragOffset;

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(originalParent, false);
        RectTransform rt = GetComponent<RectTransform>();
        rt.localPosition = Vector3.zero; 
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
        
        transform.localScale = Vector3.one;
        canvasGroup.blocksRaycasts = true;
    }
}