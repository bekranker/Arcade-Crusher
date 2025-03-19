using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInventoryInteraction : MonoBehaviour
{
    [SerializeField] private Vector2 _interactionAreaSize; // Etkileşim alanının boyutu
    [SerializeField] private LayerMask _interactables; // Etkileşime girebilecek nesnelerin katmanı

    Player_Actions _playerActions; // Oyuncu girdilerini yönetmek için kullanılan Input System aksiyonları

    void Awake()
    {
        _playerActions = new(); // Player_Actions sınıfının yeni bir örneğini oluştur
    }

    void OnEnable()
    {
        _playerActions.Enable(); // Aksiyonları etkinleştir
        _playerActions.Player.InventoryOne.performed += InteractOne; // InventoryOne aksiyonuna InteractOne metodunu bağla
        _playerActions.Player.InventoryTwo.performed += InteractTwo; // InventoryTwo aksiyonuna InteractTwo metodunu bağla
        _playerActions.Player.InventoryThree.performed += InteractThree; // InventoryThree aksiyonuna InteractThree metodunu bağla
    }

    void OnDisable()
    {
        _playerActions.Player.InventoryOne.performed -= InteractOne; // InventoryOne aksiyonundan InteractOne metodunu çıkar
        _playerActions.Player.InventoryTwo.performed -= InteractTwo; // InventoryTwo aksiyonundan InteractTwo metodunu çıkar
        _playerActions.Player.InventoryThree.performed -= InteractThree; // InventoryThree aksiyonundan InteractThree metodunu çıkar
        _playerActions.Disable(); // Aksiyonları devre dışı bırak
    }

    private void InteractOne(InputAction.CallbackContext context)
    {
        ExecuteInteraction(typeof(IObjectInteractOne)); // IObjectInteractOne türündeki etkileşimi çalıştır
    }

    private void InteractTwo(InputAction.CallbackContext context)
    {
        ExecuteInteraction(typeof(IObjectInteractTwo)); // IObjectInteractTwo türündeki etkileşimi çalıştır
    }

    private void InteractThree(InputAction.CallbackContext context)
    {
        ExecuteInteraction(typeof(IObjectInteractThree)); // IObjectInteractThree türündeki etkileşimi çalıştır
    }

    private void ExecuteInteraction(Type interactionType)
    {
        // Etkileşim alanındaki tüm nesneleri bul
        Collider2D[] tempInteractionObjects = Physics2D.OverlapBoxAll(transform.position, _interactionAreaSize, 0, _interactables);

        if (tempInteractionObjects.Length > 0)
        {
            IBaseInventory closestInteractable = null; // En yakın etkileşimli nesne
            float closestDistance = float.MaxValue; // En yakın mesafeyi saklamak için değişken

            // Tüm nesneler arasında dolaş
            foreach (Collider2D collider in tempInteractionObjects)
            {
                // IBaseInventory arayüzünü uygulayan nesneleri kontrol et
                var interactables = collider.GetComponents<IBaseInventory>();
                foreach (var interactable in interactables)
                {
                    // Belirtilen türdeki etkileşimi kontrol et
                    if (interactionType.IsAssignableFrom(interactable.GetType()))
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
            }

            // En yakın nesneyle etkileşime geç
            if (closestInteractable != null)
            {
                closestInteractable.Execute(); // Etkileşimi çalıştır
            }
        }
    }
}