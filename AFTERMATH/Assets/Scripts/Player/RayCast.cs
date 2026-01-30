using UnityEngine;

public class RayCast : MonoBehaviour
{
    [SerializeField] GameObject interactIndicartor;
    [SerializeField] float rayDistance = 3f;
    [SerializeField] KeyCode pickUp;
    [SerializeField] LayerMask interactLayer;
    Interactable currentInteractable;
    Camera _camera;
    void Start()
    {
        _camera = Camera.main;
    }
    void Update()
    {
        Ray ray = new Ray(_camera.transform.position, _camera.transform.forward);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, rayDistance, interactLayer) && hit.collider.GetComponent<Interactable>() != null)
        {
            interactIndicartor.SetActive(true);
            if(Input.GetKeyDown(pickUp))
            {
                currentInteractable = hit.collider.gameObject.GetComponent<Interactable>();
                currentInteractable.Interact();
            }
        }
        else interactIndicartor.SetActive(false);
    }
}
