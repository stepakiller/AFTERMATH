using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.Rendering.HighDefinition;
public class ExitButtonEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Renderer targetRenderer;
    [SerializeField] Light targetAreaLight;

    [ColorUsage(false, true)] 
    [SerializeField] Color emissionHoverColor = Color.red;
    [SerializeField] Color lightHoverColor = Color.red;
    [SerializeField] float lightHoverIntensity;
    [SerializeField] Color emissionNormalColor;
    [SerializeField] float transitionDuration = 0.25f;
    [SerializeField] Ease easeType = Ease.InOutQuad;

    Color _initialEmissionColor;
    Color _initialLightColor;
    float _initialLightIntensity;
    MaterialPropertyBlock _propBlock;
    Tween _emissionTween;
    Tween _lightIntensityTween;
    HDAdditionalLightData _hdLightData;

    static readonly int EmissiveColorId = Shader.PropertyToID("_EmissiveColor");

    void Awake()
    {
        _propBlock = new MaterialPropertyBlock();
        _initialEmissionColor = emissionNormalColor;
        if (targetAreaLight != null)
        {
            _initialLightColor = targetAreaLight.color;
            _hdLightData = targetAreaLight.GetComponent<HDAdditionalLightData>();
            if (_hdLightData != null) _initialLightIntensity = _hdLightData.intensity; 
            else  _initialLightIntensity = targetAreaLight.intensity;
        }
    }

    public void OnPointerEnter(PointerEventData eventData) => AnimateTo(emissionHoverColor, lightHoverColor, lightHoverIntensity);

    public void OnPointerExit(PointerEventData eventData) => AnimateTo(_initialEmissionColor, _initialLightColor, _initialLightIntensity);

    void AnimateTo(Color targetEmission, Color targetLightColor, float targetLightIntensity)
    {
        if (targetAreaLight != null)
        {
            targetAreaLight.DOKill(); 
            targetAreaLight.DOColor(targetLightColor, transitionDuration).SetEase(easeType);
            _lightIntensityTween?.Kill(); 

            if (_hdLightData != null) _lightIntensityTween = DOTween.To(() => _hdLightData.intensity, x => _hdLightData.intensity = x, targetLightIntensity, transitionDuration).SetEase(easeType);
            else targetAreaLight.DOIntensity(targetLightIntensity, transitionDuration).SetEase(easeType);
        }

        if (targetRenderer != null)
        {
            targetRenderer.GetPropertyBlock(_propBlock);
            Color currentEmission = _propBlock.HasColor(EmissiveColorId) 
                ? _propBlock.GetColor(EmissiveColorId) 
                : _initialEmissionColor;

            _emissionTween?.Kill();
            _emissionTween = DOTween.To(
                () => currentEmission, 
                x =>                   
                {
                    currentEmission = x;
                    targetRenderer.GetPropertyBlock(_propBlock);
                    _propBlock.SetColor(EmissiveColorId, currentEmission);
                    targetRenderer.SetPropertyBlock(_propBlock);
                }, 
                targetEmission,        
                transitionDuration     
            ).SetEase(easeType);
        }
    }

    void OnDestroy()
    {
        _emissionTween?.Kill();
        _lightIntensityTween?.Kill();
        if (targetAreaLight != null) targetAreaLight.DOKill();
    }
}