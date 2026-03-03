using UnityEngine;
using System.Collections;
public class Tooltip : MonoBehaviour
{
    [SerializeField] Transform playerTransform;
    [SerializeField] float fadeStartDistance;
    [SerializeField] float disappearDistance;
    [SerializeField] float farFadeStartDistance;
    [SerializeField] float maxDistance;
    [SerializeField] float appearSpeed;

    CanvasGroup _canvasGroup;
    Transform _transform;
    Camera _mainCamera;
    Vector3 _originalScale;

    void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _transform = transform;
        _mainCamera = Camera.main;
        _originalScale = _transform.localScale; 
        _transform.localScale = Vector3.zero;
        _canvasGroup.alpha = 0f;
    }

    void OnEnable() => StartCoroutine(AppearRoutine());

    void Update() => HandleDistanceFading();

    void HandleDistanceFading()
    {
        _transform.LookAt(_transform.position + _mainCamera.transform.rotation * Vector3.forward, _mainCamera.transform.rotation * Vector3.up);

        float distance = Vector3.Distance(_transform.position, playerTransform.position);
        float nearAlpha = Mathf.InverseLerp(disappearDistance, fadeStartDistance, distance);
        float farAlpha = Mathf.InverseLerp(maxDistance, farFadeStartDistance, distance);
        float finalAlpha = Mathf.Min(nearAlpha, farAlpha);
        _canvasGroup.alpha = finalAlpha;
        if (distance < disappearDistance) gameObject.SetActive(false);
    }
    IEnumerator AppearRoutine()
    {
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * appearSpeed;
            float scaleFactor = Mathf.SmoothStep(0f, 1f, t);
            _transform.localScale = _originalScale * scaleFactor;
            yield return null;
        }
        _transform.localScale = _originalScale;
    }
}
