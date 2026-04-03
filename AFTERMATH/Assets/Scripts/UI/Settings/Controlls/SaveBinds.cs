using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Globalization;
public class SaveBinds : MonoBehaviour
{
    [SerializeField] Button applyButton;
    [SerializeField] Button resetButton;
    [SerializeField] Slider mouseSensitivitySlider;
    [SerializeField] TMP_InputField mouseSensitivityInput;
    [SerializeField] float minMouseSens = 0.1f;
    [SerializeField] float maxMouseSens = 10f;

    [Header("Gamepad Settings")]
    [SerializeField] Slider gamepadSensitivitySlider;
    [SerializeField] TMP_InputField gamepadSensitivityInput;
    [SerializeField] float minGamepadSens = 10f;
    [SerializeField] float maxGamepadSens = 300f;

    void Start()
    {
        if (applyButton != null) applyButton.onClick.AddListener(SaveSettings);
        if (resetButton != null) resetButton.onClick.AddListener(ResetSettings);

        if (mouseSensitivitySlider != null)
        {
            mouseSensitivitySlider.minValue = minMouseSens;
            mouseSensitivitySlider.maxValue = maxMouseSens;
            mouseSensitivitySlider.value = InputManager.Instance.MouseSensitivity;
            mouseSensitivitySlider.onValueChanged.AddListener(OnMouseSliderChanged);
        }

        if (mouseSensitivityInput != null)
        {
            mouseSensitivityInput.text = InputManager.Instance.MouseSensitivity.ToString("F2");
            mouseSensitivityInput.onEndEdit.AddListener(OnMouseInputChanged); 
        }

        if (gamepadSensitivitySlider != null)
        {
            gamepadSensitivitySlider.minValue = minGamepadSens;
            gamepadSensitivitySlider.maxValue = maxGamepadSens;
            gamepadSensitivitySlider.value = InputManager.Instance.GamepadSensitivity;
            gamepadSensitivitySlider.onValueChanged.AddListener(OnGamepadSliderChanged);
        }

        if (gamepadSensitivityInput != null)
        {
            gamepadSensitivityInput.text = InputManager.Instance.GamepadSensitivity.ToString("F0");
            gamepadSensitivityInput.onEndEdit.AddListener(OnGamepadInputChanged);
        }
    }

    void OnMouseSliderChanged(float value)
    {
        InputManager.Instance.MouseSensitivity = value;
        if (mouseSensitivityInput != null) mouseSensitivityInput.SetTextWithoutNotify(value.ToString("F2"));
    }

    void OnMouseInputChanged(string input)
    {
        if (float.TryParse(input.Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture, out float parsedValue))
        {
            parsedValue = Mathf.Clamp(parsedValue, minMouseSens, maxMouseSens);
            
            InputManager.Instance.MouseSensitivity = parsedValue;
            if (mouseSensitivitySlider != null) mouseSensitivitySlider.value = parsedValue;
            mouseSensitivityInput.text = parsedValue.ToString("F2");
        }
        else mouseSensitivityInput.text = InputManager.Instance.MouseSensitivity.ToString("F2");
    }

    void OnGamepadSliderChanged(float value)
    {
        InputManager.Instance.GamepadSensitivity = value;
        if (gamepadSensitivityInput != null) gamepadSensitivityInput.text = value.ToString("F0");
    }

    void OnGamepadInputChanged(string input)
    {
        if (float.TryParse(input.Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture, out float parsedValue))
        {
            parsedValue = Mathf.Clamp(parsedValue, minGamepadSens, maxGamepadSens);
            InputManager.Instance.GamepadSensitivity = parsedValue;
            if (gamepadSensitivitySlider != null) gamepadSensitivitySlider.value = parsedValue;
            gamepadSensitivityInput.text = parsedValue.ToString("F0");
        }
        else gamepadSensitivityInput.text = InputManager.Instance.GamepadSensitivity.ToString("F0");
    }
    void SaveSettings() => InputManager.Instance.SaveBindings();

    void ResetSettings()
    {
        InputManager.Instance.ResetBindings();
        RebindButton.RefreshAllUI();
        
        if (mouseSensitivitySlider != null) mouseSensitivitySlider.value = InputManager.Instance.MouseSensitivity;
        else if (mouseSensitivityInput != null) mouseSensitivityInput.text = InputManager.Instance.MouseSensitivity.ToString("F2");

        if (gamepadSensitivitySlider != null) gamepadSensitivitySlider.value = InputManager.Instance.GamepadSensitivity;
        else if (gamepadSensitivityInput != null) gamepadSensitivityInput.text = InputManager.Instance.GamepadSensitivity.ToString("F0");
    }

    void OnDestroy()
    {
        if (applyButton != null) applyButton.onClick.RemoveListener(SaveSettings);
        if (resetButton != null) resetButton.onClick.RemoveListener(ResetSettings);
        if (mouseSensitivitySlider != null) mouseSensitivitySlider.onValueChanged.RemoveListener(OnMouseSliderChanged);
        if (mouseSensitivityInput != null) mouseSensitivityInput.onEndEdit.RemoveListener(OnMouseInputChanged);
        if (gamepadSensitivitySlider != null) gamepadSensitivitySlider.onValueChanged.RemoveListener(OnGamepadSliderChanged);
        if (gamepadSensitivityInput != null) gamepadSensitivityInput.onEndEdit.RemoveListener(OnGamepadInputChanged);
    }
}