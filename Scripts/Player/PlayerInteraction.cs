using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private Vector2 _interactionAreaSize;
    [SerializeField] private LayerMask _interactables;

    Player_Actions _playerActions;

    public PlayerInteraction()
    {
    }

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
        // Etkileşim alanındaki tüm nesneleri bul
        Collider2D[] tempInteractionObjects = Physics2D.OverlapBoxAll(transform.position, _interactionAreaSize, 0, _interactables);

        if (tempInteractionObjects.Length > 0)
        {
            IObjectInteractable closestInteractable = null;
            float closestDistance = float.MaxValue;

            // Tüm nesneler arasında dolaş
            foreach (Collider2D collider in tempInteractionObjects)
            {
                // IObjectInteractable arayüzünü uygulayan nesneleri kontrol et
                if (collider.TryGetComponent(out IObjectInteractable interactable))
                {
                    // Nesnenin mesafesini hesapla
                    float distance = Vector2.Distance(transform.position, collider.transform.position);

                    // En yakın nesneyi güncelle
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestInteractable = interactable;
                    }
                }
            }

            // En yakın nesneyle etkileşime geç
            if (closestInteractable != null)
            {
                closestInteractable.ExecuteInteraction();
            }
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, _interactionAreaSize);
    }

}