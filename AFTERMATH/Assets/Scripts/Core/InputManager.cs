using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.IO;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    public PlayerControls InputActions { get; private set; }

    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }
    public bool IsSprinting { get; private set; }
    public bool IsCrouching { get; private set; }
    public bool IsMouseInput { get; private set; }

    public event Action OnJumpPressed;
    public event Action OnInteractPressed;
    public event Action OnDropPressed;
    public event Action OnPausePressed;
    public event Action OnUnpausePressed;

    string bindsSavePath;

    void Awake()
    {
        if (Instance == null) 
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else 
        {
            Destroy(gameObject);
            return;
        }

        InputActions = new PlayerControls();
        bindsSavePath = Path.Combine(Application.persistentDataPath, "keybinds.json");

        LoadBindings();

        InputActions.Player.Move.performed += ctx => MoveInput = ctx.ReadValue<Vector2>();
        InputActions.Player.Move.canceled += ctx => MoveInput = Vector2.zero;

        InputActions.Player.Look.performed += ctx => 
        {
            LookInput = ctx.ReadValue<Vector2>();
            IsMouseInput = ctx.control.device is Mouse;
        };
        InputActions.Player.Look.canceled += ctx => LookInput = Vector2.zero;

        InputActions.Player.Sprint.performed += ctx => IsSprinting = true;
        InputActions.Player.Sprint.canceled += ctx => IsSprinting = false;

        InputActions.Player.Crouch.performed += ctx => IsCrouching = true;
        InputActions.Player.Crouch.canceled += ctx => IsCrouching = false;

        InputActions.Player.Jump.performed += ctx => OnJumpPressed?.Invoke();
        InputActions.Player.Interact.performed += ctx => OnInteractPressed?.Invoke();
        InputActions.Player.Drop.performed += ctx => OnDropPressed?.Invoke();
        InputActions.Player.Pause.performed += ctx => OnPausePressed?.Invoke();
        InputActions.UI.UnPause.performed += ctx => OnUnpausePressed?.Invoke();
    }

    public void EnablePlayerInput()
    {
        InputActions.UI.Disable();
        InputActions.Player.Enable();
    }

    public void EnableUIInput()
    {
        InputActions.Player.Disable();
        InputActions.UI.Enable();
    }

    void OnEnable() => EnablePlayerInput();
    void OnDisable() => InputActions.Disable();

    public void SaveBindings()
    {
        string rebinds = InputActions.SaveBindingOverridesAsJson();
        File.WriteAllText(bindsSavePath, rebinds);
    }

    public void LoadBindings()
    {
        if (File.Exists(bindsSavePath))
        {
            string rebinds = File.ReadAllText(bindsSavePath);
            InputActions.LoadBindingOverridesFromJson(rebinds);
        }
    }

    public void ResetBindings()
    {
        InputActions.RemoveAllBindingOverrides();
        SaveBindings();
    }
}
