using UnityEngine;
using UnityEngine.InputSystem;
using TMPro; 
using UnityEngine.UI;
using System;

public class RebindButton : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI bindButtonText;  
    [SerializeField] Button rebindButton;
    [SerializeField] string actionMapName = "Player"; 
    [SerializeField] string actionName = "Jump";      
    [SerializeField] int bindingIndex = 0; 
    [SerializeField] string controlScheme = "KeyboardMouse";

    InputAction actionToRebind;
    InputActionRebindingExtensions.RebindingOperation rebindingOperation;
    public static event Action OnAnyBindingChanged;

    void Start()
    {
        actionToRebind = InputManager.Instance.InputActions.asset.FindAction($"{actionMapName}/{actionName}");

        if (actionToRebind == null) return;

        UpdateUI(); 
        rebindButton.onClick.AddListener(StartRebinding);
    }

    void OnEnable() => OnAnyBindingChanged += UpdateUI;

    void OnDisable() => OnAnyBindingChanged -= UpdateUI;

    void StartRebinding()
    {
        rebindButton.interactable = false;
        bindButtonText.text = "...";

        actionToRebind.Disable();

        rebindingOperation = actionToRebind.PerformInteractiveRebinding(bindingIndex)
            .WithBindingGroup(controlScheme)
            .WithControlsExcluding("Mouse/position") 
            .WithControlsExcluding("Mouse/delta")
            .WithCancelingThrough("<Keyboard>/escape") 
            .OnComplete(operation => RebindComplete()) 
            .OnCancel(operation => RebindCancelled()) 
            .Start();
    }

    void RebindComplete()
    {
        rebindingOperation.Dispose();
        ResolveDuplicates();
        actionToRebind.Enable();
        rebindButton.interactable = true;
        OnAnyBindingChanged?.Invoke();
    }

    void RebindCancelled()
    {
        rebindingOperation.Dispose();
        actionToRebind.Enable();
        rebindButton.interactable = true;
        UpdateUI();
    }

    void ResolveDuplicates()
    {
        string newBindingPath = actionToRebind.bindings[bindingIndex].effectivePath;

        if (string.IsNullOrEmpty(newBindingPath)) return; 

        foreach (InputAction action in InputManager.Instance.InputActions.asset)
        {
            for (int i = 0; i < action.bindings.Count; i++)
            {
                InputBinding binding = action.bindings[i];
                if (action == actionToRebind && i == bindingIndex) continue;
                if (!string.IsNullOrEmpty(binding.effectivePath) && binding.effectivePath == newBindingPath) action.ApplyBindingOverride(i, "");
            }
        }
    }

    void UpdateUI()
    {
        if (actionToRebind == null) return;
        string displayString = InputControlPath.ToHumanReadableString(actionToRebind.bindings[bindingIndex].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
        if (string.IsNullOrEmpty(displayString)) bindButtonText.text = " ";
        else bindButtonText.text = displayString;
    }

    void OnDestroy() => rebindButton.onClick.RemoveListener(StartRebinding);

    public static void RefreshAllUI() => OnAnyBindingChanged?.Invoke();
}
