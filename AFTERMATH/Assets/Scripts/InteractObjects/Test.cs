using UnityEngine;

public class Test : MonoBehaviour, Interactable
{
    public void Interact()
    {
        Bootstrapper.Inventory.PickUpObject(gameObject);
    }
}
