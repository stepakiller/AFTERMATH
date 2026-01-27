using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public GameObject CurrentObject;
    
    [SerializeField] KeyCode drop;
    [SerializeField] Transform dropPoint;
    [SerializeField] float dropForce;

    void Start()
    {
        
    }
    
    void Update()
    {
        if(Input.GetKeyDown(drop) && CurrentObject != null) DropObject();
    }
    
    public void PickUpObject(GameObject obj)
    {
        CurrentObject = obj;
        obj.SetActive(false);
    }
    
    public void DropObject()
    {
        CurrentObject.SetActive(true);
        CurrentObject.transform.position = dropPoint.position;
        CurrentObject.transform.rotation = Quaternion.identity;
        Rigidbody rb = CurrentObject.GetComponent<Rigidbody>();
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        Vector3 throwDirection = dropPoint.forward;
        rb.AddForce(throwDirection * dropForce, ForceMode.Impulse);
        CurrentObject = null;
    }
}