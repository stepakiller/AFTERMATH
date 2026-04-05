using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// БРОНЕЖИЛЕТ №1: Заставляем Unity гарантировать наличие этих компонентов
[RequireComponent(typeof(Image), typeof(CanvasGroup))]
public class UI_DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public int slotIndex;
    public ItemContainer containerType;
    
    Transform originalParent;
    Image image;
    CanvasGroup canvasGroup;
    Canvas rootCanvas; 
    RectTransform rectTransform;

    void Awake()
    {
        InitComponents();
    }

    // БРОНЕЖИЛЕТ №2: Выносим поиск компонентов в отдельный метод
    void InitComponents()
    {
        if (image == null) image = GetComponent<Image>();
        if (canvasGroup == null) canvasGroup = GetComponent<CanvasGroup>();
        if (rectTransform == null) rectTransform = GetComponent<RectTransform>();
        
        if (rootCanvas == null)
        {
            Canvas parentCanvas = GetComponentInParent<Canvas>();
            if (parentCanvas != null) rootCanvas = parentCanvas.rootCanvas;
        }
    }

    public void Setup(ItemData item, int index)
    {
        // Вызываем инициализацию ПЕРЕД настройкой. 
        // Если Awake не успел сработать (префаб был выключен), мы найдем всё прямо сейчас.
        InitComponents(); 

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
        originalParent = transform.parent;
        
        if (rootCanvas != null)
        {
            transform.SetParent(rootCanvas.transform, true); 
            transform.SetAsLastSibling();
        }
        
        canvasGroup.blocksRaycasts = false; 
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (rootCanvas != null && RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)rootCanvas.transform, 
            eventData.position, 
            eventData.pressEventCamera, 
            out Vector2 localPoint))
        {
            transform.position = rootCanvas.transform.TransformPoint(localPoint);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Возвращаем в родной слот
        transform.SetParent(originalParent, false); 
        
        // Мягко возвращаем точно в центр слота, НЕ меняя ширину и высоту
        rectTransform.localPosition = Vector3.zero; 
        rectTransform.anchoredPosition = Vector2.zero; 
        transform.localScale = Vector3.one;
        
        // Снова делаем кликабельным
        canvasGroup.blocksRaycasts = true;
    }
}