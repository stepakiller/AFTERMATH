using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class SafeController : MonoBehaviour
{
    [SerializeField] AudioSource errorSounds;
    [SerializeField] AudioClip audioClip;
    [SerializeField] string _correctPin = "1234";
    [SerializeField] List<SafeButton> _safeButtons;
    [SerializeField] List<TelephoneButton> _telephoneButtons;
    [SerializeField] Transform _doorTransform;
    [SerializeField] float _openAngle = 90f;
    [SerializeField] float _openSpeed = 2f;
    [SerializeField] ZoomObject zoomObject;
    [SerializeField] GameObject tv;
    [SerializeField] GameObject photo;
    [SerializeField] Door targetDoor;
    [SerializeField] GameObject CanvasTelephone;
    [SerializeField] GameObject[] pictures;
    [SerializeField] UnityEvent _onSafeUnlocked;
    
    string _currentInput = "";

    Quaternion _closedRotation;
    Quaternion _openRotation;

    void Start()
    {
        if (_doorTransform != null)
        {
            _closedRotation = _doorTransform.localRotation;
            _openRotation = _closedRotation * Quaternion.Euler(0f, 0f, _openAngle);
        }
    }

    void OnEnable()
    {
       if(_safeButtons != null) foreach (var button in _safeButtons) button.OnButtonPressed += HandleButtonPress;
       if(_telephoneButtons != null) foreach (var button in _telephoneButtons) button.OnButtonPressed += HandleButtonPress;
    }

    void OnDisable()
    {
        if(_safeButtons != null) foreach (var button in _safeButtons) button.OnButtonPressed -= HandleButtonPress;
        if(_telephoneButtons != null) foreach (var button in _telephoneButtons) button.OnButtonPressed -= HandleButtonPress;
    }

    void HandleButtonPress(int number)
    {
        _currentInput += number.ToString();
        Debug.Log($"Ввод: {_currentInput}");
        if (_currentInput.Length == _correctPin.Length)
        {
            if (_currentInput == _correctPin)
            {
                Debug.Log("<color=green>Дозвонились!</color>");
                _onSafeUnlocked?.Invoke();
            }
            else
            {
                Debug.Log("<color=red>Неверный номер!</color>");
                errorSounds.clip = audioClip;
                errorSounds.Play();
                _currentInput = "";
            }
        }
    }

    IEnumerator OpenDoorRoutine()
    {
        float progress = 0f;
        while (progress < 1f)
        {
            progress += Time.deltaTime * _openSpeed;
            _doorTransform.localRotation = Quaternion.Slerp(_closedRotation, _openRotation, progress);
            yield return null; 
        }
        _doorTransform.localRotation = _openRotation;
    }

    public void SafeUnlocked()
    {
        zoomObject.ExitZoomMode();
        zoomObject.gameObject.layer = 0;
        targetDoor.OpenDoor();
        StartCoroutine(OpenDoorRoutine());
    }

    public void TelephoneUnloced()
    {
        zoomObject.ExitZoomMode();
        zoomObject.gameObject.layer = 0; 
        tv.SetActive(false);
        CanvasTelephone.SetActive(false);
        photo.SetActive(true);
        for (int i = 0; i < pictures.Length; i++) pictures[i].SetActive(true);
    }
}