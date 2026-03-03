using UnityEngine;

public class Inventory : MonoBehaviour
{
    public GameObject CurrentObject { get; private set; }
    ItemSettings currentSettings;

    [Header("Настройки рук")]
    [SerializeField] Transform holdPivot;
    [SerializeField] float smoothSpeed = 15f;
    
    [Header("Настройки выброса")]
    [SerializeField] KeyCode dropKey = KeyCode.Q;
    [SerializeField] Transform dropPoint;
    [SerializeField] float dropForce = 5f;

    Vector3 originalScale;
    Collider currentCollider;
    Rigidbody rb;

    void Update()
    {
        if (Input.GetKeyDown(dropKey) && CurrentObject != null)
        {
            DropObject();
            Bootstrapper.HotbarManager.RemoveCurrentItem();
        }

        if (CurrentObject != null && currentSettings != null)
        {
            Vector3 targetPos = holdPivot.TransformPoint(currentSettings.HoldPositionOffset);
            Quaternion targetRot = holdPivot.rotation * Quaternion.Euler(currentSettings.HoldRotationOffset);

            CurrentObject.transform.position = Vector3.Lerp(CurrentObject.transform.position, targetPos, Time.deltaTime * smoothSpeed);
            CurrentObject.transform.rotation = Quaternion.Slerp(CurrentObject.transform.rotation, targetRot, Time.deltaTime * smoothSpeed);

            Vector3 targetScale = Vector3.one * currentSettings.HandScale;
            CurrentObject.transform.localScale = Vector3.Lerp(CurrentObject.transform.localScale, targetScale, Time.deltaTime * smoothSpeed);
        }
    }

    public void TakeObject(GameObject obj)
    {
        CurrentObject = obj;
        CurrentObject.SetActive(true);
        currentSettings = CurrentObject.GetComponent<ItemSettings>();

        if (CurrentObject.TryGetComponent(out Equippable equippable)) equippable.Equip();

        originalScale = CurrentObject.transform.localScale;
        
        rb = CurrentObject.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.useGravity = false;
        rb.isKinematic = true;
        currentCollider = CurrentObject.GetComponent<Collider>();
        currentCollider.enabled = false;
    }

    public void HideObjectInPocket()
    {
        if (CurrentObject == null) return;
        
        if (CurrentObject.TryGetComponent(out Equippable equippable)) equippable.Unequip();
        
        CurrentObject.SetActive(false);
        CurrentObject = null;
        currentSettings = null;
    }

    public void DropObject()
    {
        if (CurrentObject == null) return;
        if (CurrentObject.TryGetComponent(out Equippable equippable)) equippable.Unequip();

        CurrentObject.transform.localScale = originalScale;
        CurrentObject.transform.position = dropPoint.position;
        CurrentObject.transform.rotation = Quaternion.identity;

        rb.isKinematic = false;
        rb.useGravity = true;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        currentCollider.enabled = true;

        Vector3 throwDirection = dropPoint.forward;
        rb.AddForce(throwDirection * dropForce, ForceMode.Impulse);
        
        CurrentObject = null;
        currentSettings = null;
    }
}