using UnityEngine;

public class Cassette : MonoBehaviour, Interactable
{
    [SerializeField] Door targetDoor;
    [SerializeField] GameObject Cassette1;
    [SerializeField] DictafonController dictafonController;
    [SerializeField] AudioClip newSound;
    public void Interact()
    {
        Cassette1.SetActive(false);
        dictafonController.audioClip = newSound;
        Destroy(gameObject);
        targetDoor.OpenDoor();
    }
}
