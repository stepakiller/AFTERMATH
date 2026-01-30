using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition; // Обязательно для HDRP

public class CameraAutoFocus : MonoBehaviour
{
    [Header("Setup")]
    public Volume globalVolume;
    public LayerMask focusLayer;

    [Header("Settings")]
    public float focusSpeed = 5f;
    public float maxFocusDistance = 50f;
    public float minFocusDistance = 0.5f;

    [Header("Blur Settings (Manual Ranges)")]
    [Tooltip("Ширина зоны резкости. Чем меньше - тем сложнее поймать фокус.")]
    public float focusWidth = 0.5f; // "Толщина" фокуса
    [Tooltip("Как быстро начинается мыло после зоны резкости.")]
    public float falloff = 5.0f; 

    private DepthOfField dof;
    private float targetDistance;
    private float currentFocusDist;

    void Start()
    {
        if (globalVolume.profile.TryGet<DepthOfField>(out dof))
        {
            // Активируем нужные нам ползунки в Volume
            dof.nearFocusStart.overrideState = true;
            dof.nearFocusEnd.overrideState = true;
            dof.farFocusStart.overrideState = true;
            dof.farFocusEnd.overrideState = true;
        }
        else
        {
            Debug.LogError("Depth Of Field не найден в Global Volume!");
        }
    }

    void Update()
    {
        if (dof == null) return;

        // 1. РЕЙКАСТ
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxFocusDistance, focusLayer, QueryTriggerInteraction.Ignore))
        {
            targetDistance = hit.distance;
        }
        else
        {
            targetDistance = maxFocusDistance;
        }
        
        // Ограничение минимальной дистанции
        targetDistance = Mathf.Max(targetDistance, minFocusDistance);

        // 2. ПЛАВНАЯ ИНТЕРПОЛЯЦИЯ (LERP)
        // Мы двигаем не сами ползунки, а виртуальную точку фокуса
        currentFocusDist = Mathf.Lerp(currentFocusDist, targetDistance, Time.deltaTime * focusSpeed);

        // 3. ПРИМЕНЕНИЕ К MANUAL RANGES
        // Настраиваем "Ближний диапазон" (размытие перед носом)
        dof.nearFocusStart.value = 0f;
        dof.nearFocusEnd.value = Mathf.Max(0f, currentFocusDist - focusWidth);

        // Настраиваем "Дальний диапазон" (размытие фона)
        dof.farFocusStart.value = currentFocusDist + focusWidth;
        dof.farFocusEnd.value = currentFocusDist + focusWidth + falloff;
    }
}
