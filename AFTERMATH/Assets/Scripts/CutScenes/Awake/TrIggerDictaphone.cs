using UnityEngine;
using UnityEngine.Playables;

public class TriggerDictaphone : MonoBehaviour
{
    [SerializeField] PlayableDirector director;
    [SerializeField] GameObject timeline;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            timeline.SetActive(true);
            director.Play();
            Destroy(gameObject);
        }
    }
}
