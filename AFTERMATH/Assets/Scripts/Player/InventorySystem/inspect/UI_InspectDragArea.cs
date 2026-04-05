using UnityEngine;
using UnityEngine.EventSystems;

public class UI_InspectDragArea : MonoBehaviour, IDragHandler
{
    [Header("Настройки вращения")]
    public float rotationSpeed = 0.5f;
    
    // НОВОЕ: Добавляем ссылку на камеру нашей фотостудии
    public Camera inspectCamera; 

    [HideInInspector] public Transform objectToRotate; 

    public void OnDrag(PointerEventData eventData)
    {
        // Проверяем, что камера назначена
        if (objectToRotate != null && inspectCamera != null)
        {
            float rotX = eventData.delta.y * rotationSpeed;
            float rotY = -eventData.delta.x * rotationSpeed;

            // ИСПРАВЛЕНИЕ: Теперь мы вращаем предмет относительно осей Inspect Camera
            objectToRotate.Rotate(inspectCamera.transform.up, rotY, Space.World);
            objectToRotate.Rotate(inspectCamera.transform.right, rotX, Space.World);
        }
    }
}
