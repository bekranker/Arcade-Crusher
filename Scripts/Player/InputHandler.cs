using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    private Player_Actions _playerActions;

    public static event Action<InputAction.CallbackContext> DebugClick, MoveAction, InteractAction;


    void Awake()
    {
        _playerActions = new();
    }
    void OnEnable()
    {
        _playerActions.Player.Move.performed += MoveAction;
        _playerActions.Player.Move.canceled += MoveAction;

        _playerActions.Player.Debug.performed += DebugClick;
        _playerActions.Player.Debug.performed += InteractAction;
    }
    void OnDisable()
    {
        _playerActions.Player.Move.performed -= MoveAction;
        _playerActions.Player.Move.canceled -= MoveAction;

        _playerActions.Player.Debug.performed -= DebugClick;
        _playerActions.Player.Debug.performed -= InteractAction;

    }
}