using UnityEngine;

public class UIPulser : MonoBehaviour
{
    [SerializeField] float pulseSpeed = 1.5f;
    [SerializeField] float minAlpha = 0.2f;
    [SerializeField] float maxAlpha = 1.0f;
    CanvasGroup _canvasGroup;

    void Awake() => _canvasGroup = GetComponent<CanvasGroup>();

    void Update()
    {
        float pingPongValue = Mathf.PingPong(Time.time * pulseSpeed, 1f);
        _canvasGroup.alpha = Mathf.Lerp(minAlpha, maxAlpha, pingPongValue);
    }
}
