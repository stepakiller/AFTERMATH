using UnityEngine;
using UnityEngine.Rendering;
public class WakeUpEffect : MonoBehaviour
{
    [SerializeField] float duration;
    Volume awakeVolume;

    void Start()
    {
        awakeVolume = GetComponent<Volume>();
        awakeVolume.weight = 1f;
    }

    void Update()
    {
        if (awakeVolume.weight > 0) awakeVolume.weight -= Time.deltaTime / duration;
    }
}
