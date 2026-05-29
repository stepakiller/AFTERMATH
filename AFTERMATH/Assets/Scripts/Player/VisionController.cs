using System.Collections;
using UnityEngine;

public class VisionController : MonoBehaviour
{
    [Header("Камеры")]
    [Tooltip("Твоя обычная основная камера")]
    [SerializeField] GameObject _mainCameraObj;
    [Tooltip("Новая камера для пустоты")]
    [SerializeField] GameObject _visionCameraObj;
    
    [Header("UI и Эффекты")]
    [Tooltip("UI CanvasGroup с черным фоном")]
    [SerializeField] CanvasGroup _blinkOverlay;
    [SerializeField] float _blinkSpeed = 5f;
    [SerializeField] AudioClip _visionToggleSound;

    AudioSource _audioSource;
    bool _isVisionActive = false;
    Coroutine _transitionCoroutine;

    void Awake()
    {

        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null && _visionToggleSound != null)
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
            _audioSource.playOnAwake = false;
        }
    }

    void Start()
    {
        if (_blinkOverlay != null) _blinkOverlay.alpha = 0f;
        _visionCameraObj.SetActive(false);
        _mainCameraObj.SetActive(true);
        if (InputManager.Instance != null) InputManager.Instance.OnVisionPressed += ToggleVision;
    }

    private void OnDestroy()
    {
        if (InputManager.Instance != null) InputManager.Instance.OnVisionPressed -= ToggleVision;
    }

    public void ForceDisableVision()
    {
        if (_isVisionActive) ToggleVision();
    }

    private void ToggleVision()
    {
        _isVisionActive = !_isVisionActive;
        if (_transitionCoroutine != null) StopCoroutine(_transitionCoroutine);
        _transitionCoroutine = StartCoroutine(BlinkTransitionRoutine());
    }

    private IEnumerator BlinkTransitionRoutine()
    {
        while (_blinkOverlay.alpha < 1f)
        {
            _blinkOverlay.alpha += Time.deltaTime * _blinkSpeed;
            yield return null;
        }
        
        _blinkOverlay.alpha = 1f;

        if (_isVisionActive)
        {
            _mainCameraObj.SetActive(false);
            _visionCameraObj.SetActive(true);
        }
        else
        {
            _visionCameraObj.SetActive(false);
            _mainCameraObj.SetActive(true);
        }

        if (_visionToggleSound != null && _audioSource != null) _audioSource.PlayOneShot(_visionToggleSound);

        while (_blinkOverlay.alpha > 0f)
        {
            _blinkOverlay.alpha -= Time.deltaTime * _blinkSpeed;
            yield return null;
        }

        _blinkOverlay.alpha = 0f;
    }
}