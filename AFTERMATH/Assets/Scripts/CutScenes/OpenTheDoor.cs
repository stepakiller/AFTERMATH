using UnityEngine;

public class OpenTheDoor : MonoBehaviour, Interactable
{
    [SerializeField] GameObject timeLime3;
    public void Interact()
    {
        timeLime3.SetActive(true);
    }
}
