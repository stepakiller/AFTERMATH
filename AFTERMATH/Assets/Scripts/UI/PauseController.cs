using UnityEngine;
using UnityEngine.Rendering;
using DG.Tweening;

public class PauseController : MonoBehaviour
{
    [SerializeField] GameObject pauseScreen; 
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] Volume globalVolume;
    [SerializeField] Volume pauseVolume;
    [SerializeField] Material blurMaterial;
    [SerializeField] float targetBlurValue = 2f;
    [SerializeField] float fadeDuration = 0.3f;
    [SerializeField] float globalVolumeTargetWeight;
    string blurParameterName = "_Blur";

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
        if (blurMaterial != null) blurMaterial.SetFloat(blurParameterName, 0f);
    }

    void PauseGame()
    {
        Time.timeScale = 0f;
        pauseScreen.SetActive(true);
        canvasGroup.alpha = 0f;
        canvasGroup.DOFade(1f, fadeDuration).SetUpdate(true);

        if (blurMaterial != null)blurMaterial.DOFloat(targetBlurValue, blurParameterName, fadeDuration).SetUpdate(true);

        if (pauseVolume != null) DOTween.To(() => pauseVolume.weight, x => pauseVolume.weight = x, 1f, fadeDuration).SetUpdate(true);
        
        if (globalVolume != null) DOTween.To(() => globalVolume.weight, x => globalVolume.weight = x, globalVolumeTargetWeight, fadeDuration).SetUpdate(true);

        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        InputManager.Instance.EnableUIInput();
    }

    public void ResumeGame()
    {
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        if (blurMaterial != null) blurMaterial.DOFloat(0f, blurParameterName, fadeDuration).SetUpdate(true);

        if (pauseVolume != null)  DOTween.To(() => pauseVolume.weight, x => pauseVolume.weight = x, 0f, fadeDuration).SetUpdate(true);
            
        if (globalVolume != null) DOTween.To(() => globalVolume.weight, x => globalVolume.weight = x, 1f, fadeDuration).SetUpdate(true);

        canvasGroup.DOFade(0f, fadeDuration).SetUpdate(true).OnComplete(() => 
        {
            pauseScreen.SetActive(false); 
            Time.timeScale = 1f;            
        });

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        InputManager.Instance.EnablePlayerInput();
    }
}