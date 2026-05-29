using System;
using System.Collections;
using UnityEngine;

public class SafeButton : MonoBehaviour
{
    [SerializeField] int _numberValue;
    [SerializeField] float _pressDepth = 0.05f;
    [SerializeField] float _animationSpeed = 15f;

    public event Action<int> OnButtonPressed;

    Vector3 _startLocalPosition;
    bool _isAnimating = false;

    void Start() => _startLocalPosition = transform.localPosition;

    public void Interact()
    {
        if (_isAnimating) return;
        StartCoroutine(AnimatePressRoutine());
        OnButtonPressed?.Invoke(_numberValue);
    }

    IEnumerator AnimatePressRoutine()
    {
        _isAnimating = true;
        Vector3 pressedPosition = _startLocalPosition + Vector3.right * _pressDepth;
        while (Vector3.Distance(transform.localPosition, pressedPosition) > 0.001f)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, pressedPosition, Time.deltaTime * _animationSpeed);
            yield return null;
        }
        while (Vector3.Distance(transform.localPosition, _startLocalPosition) > 0.001f)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, _startLocalPosition, Time.deltaTime * _animationSpeed);
            yield return null;
        }
        transform.localPosition = _startLocalPosition;
        _isAnimating = false;
    }
}
