using UnityEngine;
using DG.Tweening;

public class SettingsButton : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private RectTransform canvasRect; // Ссылка на сам Canvas
    [SerializeField] private RectTransform pauseMenu;
    [SerializeField] private RectTransform settingsMenu;

    [Header("Animation Settings")]
    [SerializeField] private float animationDuration = 0.5f;
    [SerializeField] private Ease animationEase = Ease.OutBack;

    // Динамические позиции
    private Vector2 pauseMenuCenterPos;
    private float pauseMenuHiddenPosX;
    private float settingsMenuHiddenPosY;

    private void Start()
    {
        // 1. Запоминаем центр
        pauseMenuCenterPos = pauseMenu.anchoredPosition;

        // 2. Высчитываем идеальные позиции за кадром для ЛЮБОГО разрешения
        // Половина ширины канваса + половина ширины самой панели = точная позиция прямо за левым краем
        pauseMenuHiddenPosX = -(canvasRect.rect.width / 2f) - (pauseMenu.rect.width / 2f);
        
        // Половина высоты канваса + половина высоты панели = точная позиция прямо за верхним краем
        settingsMenuHiddenPosY = canvasRect.rect.height + (settingsMenu.rect.height / 2f);

        // 3. Прячем настройки на вычисленную высоту
        settingsMenu.anchoredPosition = new Vector2(0f, settingsMenuHiddenPosY);
    }

    public void OpenSettings()
    {
        pauseMenu.DOAnchorPosX(pauseMenuHiddenPosX, animationDuration)
                 .SetEase(animationEase)
                 .SetUpdate(true);

        settingsMenu.DOAnchorPosY(0f, animationDuration)
                    .SetEase(animationEase)
                    .SetUpdate(true);
    }

    public void CloseSettings()
    {
        pauseMenu.DOAnchorPos(pauseMenuCenterPos, animationDuration)
                 .SetEase(animationEase)
                 .SetUpdate(true);

        settingsMenu.DOAnchorPosY(settingsMenuHiddenPosY, animationDuration)
                    .SetEase(animationEase)
                    .SetUpdate(true);
    }
}