using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractionNearBy : MonoBehaviour
{
    [SerializeField] private Vector2 _interactionAreaSize;
    [SerializeField] private LayerMask _interactables;

    Collider2D[] _tempInteractionObjects;
    Collider2D[] _previousTempInteractionObjects;
    void Update()
    {
        Interact();
    }
    private void Interact()
    {
        // Etkileşim alanındaki tüm nesneleri bul
        _tempInteractionObjects = Physics2D.OverlapBoxAll(transform.position, _interactionAreaSize, 0, _interactables);

        if (_tempInteractionObjects.Length > 0)
        {
            IObjectInteractableNearBy closestInteractable = null;
            float closestDistance = float.MaxValue;

            // Tüm nesneler arasında dolaş
            foreach (Collider2D collider in _tempInteractionObjects)
            {
                // IObjectInteractable arayüzünü uygulayan nesneleri kontrol et
                if (collider.TryGetComponent(out IObjectInteractableNearBy interactable))
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
            _previousTempInteractionObjects = _tempInteractionObjects;
            // En yakın nesneyle etkileşime geç
            if (closestInteractable != null)
            {
                closestInteractable.ExecuteNearInteraction();
            }
            else
            {
                ExitAction();
            }
        }
        else
        {
            ExitAction();
        }
    }
    private void ExitAction()
    {
        if (_previousTempInteractionObjects == null) return;
        if (_previousTempInteractionObjects.Length == 0) return;
        for (int i = 0; i < _previousTempInteractionObjects.Length; i++)
        {
            if (_previousTempInteractionObjects[i] != null)
            {
                if (_previousTempInteractionObjects[i].gameObject.TryGetComponent<IObjectInteractableNearBy>(out IObjectInteractableNearBy objectInteractableNearBy))
                {
                    objectInteractableNearBy.ExitArea();
                    _previousTempInteractionObjects[i] = null;
                }
            }
        }
        _previousTempInteractionObjects = null;
    }
}