using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class LoadingScreen : MonoBehaviour
{
    [HideInInspector] public static int sceneToLoadIndex;
    [SerializeField] Image progressBarFill; 
    [SerializeField] TextMeshProUGUI progressText;
    [SerializeField] TextMeshProUGUI pressAnyKeyText;
    [SerializeField] float pulseSpeed = 1.5f; 
    [SerializeField] CanvasGroup blackScreen; 
    [SerializeField] CanvasGroup loadingUIGroup; 
    [SerializeField] float fadeDuration = 1f;
    
    bool _readyToActivate = false;
    bool _isTransitioning = false;
    IDisposable _anyButtonListener;

    void Start()
    {
        pressAnyKeyText.color = new Color(pressAnyKeyText.color.r, pressAnyKeyText.color.g, pressAnyKeyText.color.b, 0f);
        
        pressAnyKeyText.gameObject.SetActive(false);
        
        progressBarFill.fillAmount = 0f; 
        blackScreen.alpha = 1f;
        loadingUIGroup.alpha = 0f; 

        InputManager.Instance.EnableUIInput();
        System.GC.Collect(); 
        
        StartCoroutine(LoadLevelSequence());
    }

    void OnEnable() => _anyButtonListener = InputSystem.onAnyButtonPress.Call(OnAnyButtonPressed);
    void OnDisable() => _anyButtonListener?.Dispose();

    private void OnAnyButtonPressed(InputControl control)
    {
        if (!_readyToActivate) return;
        HandleSubmit();
    }

    void HandleSubmit() => _readyToActivate = true;

    IEnumerator LoadLevelSequence()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneToLoadIndex);
        operation.allowSceneActivation = false;

        yield return blackScreen.DOFade(0f, fadeDuration).WaitForCompletion();
        yield return loadingUIGroup.DOFade(1f, fadeDuration).WaitForCompletion();

        float visualProgress = 0f;
        int lastPercent = -1;
        
        while (!operation.isDone)
        {
            float targetProgress = Mathf.Clamp01(operation.progress / 0.9f);
            visualProgress = Mathf.MoveTowards(visualProgress, targetProgress, 3f * Time.unscaledDeltaTime);
            int currentPercent = Mathf.RoundToInt(visualProgress * 100f);

            if (currentPercent != lastPercent)
            {
                progressBarFill.fillAmount = visualProgress;
                progressText.SetText("{0}%", currentPercent); 
                lastPercent = currentPercent;
            }

            if (operation.progress >= 0.9f && visualProgress >= 1f)
            {
                if (!_readyToActivate && !_isTransitioning) 
                {
                    _readyToActivate = true; 
                    pressAnyKeyText.gameObject.SetActive(true);
                    float tweenDuration = 1f / pulseSpeed;
                    pressAnyKeyText.DOFade(1f, tweenDuration).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine).SetUpdate(true);
                }

                if (_readyToActivate && _isTransitioning) 
                {
                    pressAnyKeyText.DOKill();
                    InputManager.Instance.EnablePlayerInput(); 
                    yield return loadingUIGroup.DOFade(0f, fadeDuration).WaitForCompletion();
                    yield return blackScreen.DOFade(1f, fadeDuration).WaitForCompletion();
                    operation.allowSceneActivation = true; 
                }
            }
            yield return null; 
        }
    }
}