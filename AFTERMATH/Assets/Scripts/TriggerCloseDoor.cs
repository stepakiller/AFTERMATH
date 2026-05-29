using UnityEngine;

public class TriggerCloseDoor : MonoBehaviour
{
    [SerializeField] Door door;
    [SerializeField] GameObject newCoridor;
    [SerializeField] GameObject oldCoridor;
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            door.CloseDoor2();
            Invoke("Replace", 2);
        }
    }

    void Replace()
    {
        newCoridor.SetActive(true);
        oldCoridor.SetActive(false);
        Destroy(gameObject);
    }
}
