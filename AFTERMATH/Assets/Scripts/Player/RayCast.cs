using UnityEngine;
using UnityEngine.UI;

public class RayCast : MonoBehaviour
{
    [Header("Crosshair Settings")]
    [SerializeField] Image crosshairImage; 
    [SerializeField] float defaultAlpha = 0.5f;
    [SerializeField] float activeAlpha = 1f;
    [SerializeField] Vector3 defaultScale = Vector3.one; 
    [SerializeField] Vector3 activeScale = new Vector3(1.2f, 1.2f, 1.2f); 
    [Header("Raycast Settings")]
    [SerializeField] float rayDistance = 3f;
    [SerializeField] LayerMask interactLayer;
    [SerializeField] KeyCode pickUpKey = KeyCode.E;

    Camera _camera;
    bool _isLookingAtInteractable = false;

    void Start()
    {
        _camera = Camera.main;
        SetCrosshairVisuals(defaultAlpha, defaultScale); 
    }

    void Update()
    {
        Ray ray = new Ray(_camera.transform.position, _camera.transform.forward);
        
        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance, interactLayer) && hit.collider.TryGetComponent(out Interactable interactable))
        {
            if (!_isLookingAtInteractable)
            {
                SetCrosshairVisuals(activeAlpha, activeScale);
                _isLookingAtInteractable = true;
            }
            if (Input.GetKeyDown(pickUpKey)) interactable.Interact();
        }
        else
        {
            if (_isLookingAtInteractable)
            {
                SetCrosshairVisuals(defaultAlpha, defaultScale);
                _isLookingAtInteractable = false;
            }
        }
    }

    void SetCrosshairVisuals(float alpha, Vector3 targetScale)
    {
        if (crosshairImage != null)
        {
            Color crosshairColor = crosshairImage.color;
            crosshairColor.a = alpha;
            crosshairImage.color = crosshairColor;
            crosshairImage.rectTransform.localScale = targetScale;
        }
    }
}
