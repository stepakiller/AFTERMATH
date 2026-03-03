using UnityEngine;

public class PickUpItem : MonoBehaviour, Interactable
{
    public void Interact()
    {
        Bootstrapper.HotbarManager.PickupItem(gameObject);
    }
}
