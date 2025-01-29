using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private Vector2 _interactionAreaSize;
    [SerializeField] private LayerMask _interactables;

    Player_Actions _playerActions;
    void Awake()
    {
        _playerActions = new();
    }
    void OnEnable()
    {
        _playerActions.Player.Interact.performed += Interact;
        _playerActions.Player.Interact.canceled += Interact;

        _playerActions.Enable();
    }
    void OnDisable()
    {
        _playerActions.Player.Interact.performed -= Interact;
        _playerActions.Player.Interact.canceled -= Interact;

        _playerActions.Disable();
    }
    private void Interact(InputAction.CallbackContext context)
    {
        Collider2D tempInteractionObject = Physics2D.OverlapBox(transform.position, _interactionAreaSize, 0, _interactables);
        if (tempInteractionObject != null)
        {
            if (tempInteractionObject.TryGetComponent(out IObjectInteractable item))
            {
                item.ExecuteInteraction();
            }
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, _interactionAreaSize);
    }

}