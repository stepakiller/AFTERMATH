using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private float _interactDistance = 3f;
    
    [SerializeField] private LayerMask _interactableLayer; 

    private void Update()
    {
        // 0 - левая кнопка мыши
        if (Input.GetMouseButtonDown(0))
        {
            PerformRaycast();
        }
    }

    private void PerformRaycast()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, _interactDistance, _interactableLayer))
        {
            if (hit.collider.TryGetComponent(out SafeButton button1)) button1.Interact();
            else if (hit.collider.TryGetComponent(out TelephoneButton button2)) button2.Interact();
        }
    }
}