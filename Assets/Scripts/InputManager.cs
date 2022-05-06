using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    [HideInInspector] public PlayerInput Input = null;
    [HideInInspector] public bool IsPaused = false;
    [HideInInspector] public bool UsingMouseAndKeyboard = true;
    [HideInInspector] public InputAction MoveAction = null;
    [HideInInspector] public InputAction AimAction = null;
    [HideInInspector] public InputAction Fire1Action = null;
    [HideInInspector] public InputAction Fire2Action = null;

    [HideInInspector] public static UnityEvent PauseEvent = new UnityEvent();
    [HideInInspector] public static UnityEvent UnpauseEvent = new UnityEvent();
    [HideInInspector] public static UnityEvent MouseAndKeyBoardEnabled = new UnityEvent();
    [HideInInspector] public static UnityEvent GamepadEnabled = new UnityEvent();
    [HideInInspector] public static UnityEvent RestartEvent = new UnityEvent();

    private PlayerController _player = null;

    private void Awake ()
    {
        Instance = this;

        Input = GetComponent<PlayerInput>();
        MoveAction = Input.actions["Move"];
        AimAction = Input.actions["Look"];
        Fire1Action = Input.actions["Fire1"];
        Fire2Action = Input.actions["Fire2"];
    }

    private void Start ()
    {
        _player = PlayerController.Instance;
    }

    private void OnDestroy ()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void OnPause ()
    {
        IsPaused = !IsPaused;
        if (IsPaused)
        {
            Input.SwitchCurrentActionMap("UI");
            PauseEvent?.Invoke();
        }
        else
        {
            Input.SwitchCurrentActionMap("Player");
            UnpauseEvent?.Invoke();
        }
    }

    private void OnControlsChanged (PlayerInput input)
    {
        bool NowUsingMouseAndKeyboard = input.currentControlScheme == "Keyboard&Mouse";
        if (UsingMouseAndKeyboard != NowUsingMouseAndKeyboard)
        {
            if (NowUsingMouseAndKeyboard)
            {
                MouseAndKeyBoardEnabled?.Invoke();
            }
            else
            {
                GamepadEnabled?.Invoke();
            }
        }
        UsingMouseAndKeyboard = NowUsingMouseAndKeyboard;
        Cursor.visible = UsingMouseAndKeyboard;
    }

    private void OnUpgrade1 ()
    {
        _player.LevelingSystem.ApplyUpgrade(0);
    }

    private void OnUpgrade2 ()
    {
        _player.LevelingSystem.ApplyUpgrade(1);
    }

    private void OnUpgrade3 ()
    {
        _player.LevelingSystem.ApplyUpgrade(2);
    }

    private void OnReroll ()
    {

    }

    private void OnRestart ()
    {
        RestartEvent?.Invoke();
    }
}
