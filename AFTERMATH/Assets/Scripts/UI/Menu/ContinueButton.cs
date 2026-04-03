using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ContinueButton : MonoBehaviour
{
    [SerializeField] CanvasGroup _canvasGroup;
    [SerializeField] Transform _cameraTransform;
    [SerializeField] float _fadeDuration;
    [SerializeField] float _cameraDuration;
    [SerializeField] Ease _animationEase = Ease.InOutQuad;
    [SerializeField] int _targetLevelIndex;
    public void StartGame()
    {
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;

        Sequence sequence = DOTween.Sequence();
        sequence.Join(_canvasGroup.DOFade(0f, _fadeDuration).SetEase(_animationEase));
        
        sequence.Join(_cameraTransform.DORotate(new Vector3(-70f, _cameraTransform.eulerAngles.y, _cameraTransform.eulerAngles.z), _cameraDuration).SetEase(_animationEase));

        sequence.OnComplete(() =>
        {
            LoadingScreen.sceneToLoadIndex = _targetLevelIndex;
            SceneManager.LoadScene(1);
        });
    }
}