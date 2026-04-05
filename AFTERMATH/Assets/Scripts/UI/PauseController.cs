using UnityEngine;
using UnityEngine.Rendering;
using DG.Tweening;

public class PauseController : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] GameObject pauseScreen;
    [SerializeField] CanvasGroup canvasGroup;
    
    [Header("Crosshair")]
    [SerializeField] GameObject crosshair;  
    [SerializeField] CanvasGroup crosshairCanvasGroup;
    [SerializeField] float crosshairFadeDuration = 0.2f;

    [Header("Settings")]
    [SerializeField] SettingsButton settingsButton;
    [SerializeField] float fadeDuration = 0.3f;

    [Header("Volumes & Effects")]
    [SerializeField] Volume globalVolume;
    [SerializeField] float globalVolumeTargetWeight = 0.2f;
    [SerializeField] Volume pauseVolume;
    [SerializeField] Material blurMaterial;
    [SerializeField] float targetBlurValue = 2f;
    readonly int blurPropertyId = Shader.PropertyToID("_Blur");

    void Awake()
    {
        if (canvasGroup == null) canvasGroup = GetComponent<CanvasGroup>();
        InitState();
    }

    void Start()
    {
        InputManager.Instance.OnPausePressed += PauseGame;
        InputManager.Instance.OnUnpausePressed += ResumeGame;
    }

    void OnDestroy()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.OnPausePressed -= PauseGame;
            InputManager.Instance.OnUnpausePressed -= ResumeGame;
        }
    }

    void InitState()
    {
        pauseScreen.SetActive(false);
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        
        if (pauseVolume != null) pauseVolume.weight = 0f;
        if (globalVolume != null) globalVolume.weight = 1f;
        if (blurMaterial != null) blurMaterial.SetFloat(blurPropertyId, 0f);
    }

    void PauseGame()
    {
        KillAllTweens();
        Time.timeScale = 0f;
        pauseScreen.SetActive(true);
        crosshair.SetActive(false);
        if (crosshairCanvasGroup != null) crosshairCanvasGroup.alpha = 0f; 

        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 0f;
        canvasGroup.DOFade(1f, fadeDuration).SetUpdate(true);

        if (blurMaterial != null) blurMaterial.DOFloat(targetBlurValue, blurPropertyId, fadeDuration).SetUpdate(true);
        if (pauseVolume != null) DOTween.To(() => pauseVolume.weight, x => pauseVolume.weight = x, 1f, fadeDuration).SetUpdate(true);
        if (globalVolume != null) DOTween.To(() => globalVolume.weight, x => globalVolume.weight = x, globalVolumeTargetWeight, fadeDuration).SetUpdate(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        InputManager.Instance.EnableUIInput();
    }

    public void ResumeGame()
    {
        if (settingsButton.settingsIsOpen) 
        { 
            settingsButton.CloseSettings();
            return; 
        }
        KillAllTweens();

        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        if (blurMaterial != null) blurMaterial.DOFloat(0f, blurPropertyId, fadeDuration).SetUpdate(true);
        if (pauseVolume != null)  DOTween.To(() => pauseVolume.weight, x => pauseVolume.weight = x, 0f, fadeDuration).SetUpdate(true);
        if (globalVolume != null) DOTween.To(() => globalVolume.weight, x => globalVolume.weight = x, 1f, fadeDuration).SetUpdate(true);

        canvasGroup.DOFade(0f, fadeDuration).SetUpdate(true).OnComplete(() => 
        {
            pauseScreen.SetActive(false); 
            Time.timeScale = 1f;            
            crosshair.SetActive(true);
            if (crosshairCanvasGroup != null)
            {
                crosshairCanvasGroup.alpha = 0f;
                crosshairCanvasGroup.DOFade(1f, crosshairFadeDuration); 
            }
        });
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        InputManager.Instance.EnablePlayerInput();
    }

    private void KillAllTweens()
    {
        DOTween.Kill(canvasGroup);
        if (crosshairCanvasGroup != null) DOTween.Kill(crosshairCanvasGroup);
        if (blurMaterial != null) DOTween.Kill(blurMaterial);
    }
}