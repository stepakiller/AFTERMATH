using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour
{
     public GameObject CurrentObject;

    [SerializeField] Transform holdPivot;
    [SerializeField] float smoothSpeed;
    
    [SerializeField] KeyCode drop;
    [SerializeField] Transform dropPoint;
    [SerializeField] float dropForce;

    Vector3 originalScale;

    void Start()
    {
        
    }
    
    void Update()
    {
        if(Input.GetKeyDown(drop) && CurrentObject != null) DropObject();
        
        if(CurrentObject != null)
        {
            ItemSettings settings = CurrentObject.GetComponent<ItemSettings>();
            Vector3 targetPos = holdPivot.TransformPoint(settings.HoldPositionOffset);
            Quaternion targetRot = holdPivot.rotation * Quaternion.Euler(settings.HoldRotationOffset);

            CurrentObject.transform.position = Vector3.Lerp(CurrentObject.transform.position, targetPos, Time.deltaTime * smoothSpeed);
            CurrentObject.transform.rotation = Quaternion.Slerp(CurrentObject.transform.rotation, targetRot, Time.deltaTime * smoothSpeed);

            Vector3 targetScale = Vector3.one * settings.HandScale;
            CurrentObject.transform.localScale = Vector3.Lerp(CurrentObject.transform.localScale, targetScale, Time.deltaTime * smoothSpeed);
        }
    }
    
    public void PickUpObject(GameObject obj)
    {
        CurrentObject = obj;
        originalScale = CurrentObject.transform.localScale;
        Rigidbody rb = CurrentObject.GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.linearVelocity = Vector3.zero;
        rb.isKinematic = true;
        rb.angularVelocity = Vector3.zero;
        CurrentObject.GetComponent<Collider>().enabled = false;
    }
    
    public void DropObject()
    {
        CurrentObject.transform.localScale = originalScale;
        CurrentObject.transform.position = dropPoint.position;
        CurrentObject.transform.rotation = Quaternion.identity;

        Rigidbody rb = CurrentObject.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.useGravity = true;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        CurrentObject.GetComponent<Collider>().enabled = true;

        Vector3 throwDirection = dropPoint.forward;
        rb.AddForce(throwDirection * dropForce, ForceMode.Impulse);
        CurrentObject = null;
    }
}