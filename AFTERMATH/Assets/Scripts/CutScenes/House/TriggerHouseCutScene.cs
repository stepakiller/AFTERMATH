using UnityEngine;

public class TriggerHouseCutScene : MonoBehaviour
{
    [SerializeField] GameObject timeLine;
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            timeLine.SetActive(true);
            Destroy(gameObject);
        }
    }
}
