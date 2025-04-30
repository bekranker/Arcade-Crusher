using UnityEngine;
using UnityEngine.InputSystem;
using System;
using Random = UnityEngine.Random;
using DG.Tweening;
using System.Collections.Generic;


/// <summary>
/// * ilk jumpi başlatabilir
/// * anlık yüksekliği tutabilir
/// * yavaşlatma ve hızlandırma efektlerini tutabilir
/// * yavaşlatma ve hızlandırma efektlerini uygulayabilir
/// * eğer sıçrayacağı yer ekranın boyutunu geçiyorsa efekt uygulanabilir
/// * parallax efektinin hızını değiştirerek jump efektini buradan verebiliriz.
/// 
/// * zıplama inputunu ilk aldığımızda oyunculardan birini random seçer ve onu parallaxın child objesi olmasından çıkarır
///     - ayrıca zıplama animasyonunuda başlatır.
///     - geri yere düştüğünde tekrar chil objesi olur ve oturma animasyonu oynar (eğer ki yanmadıysa)
///         -- yanarsa ölme animasyonu oynar ve lose screen gelir
///         
/// 
/// </summary>
public class TwinsToTheMoonHandler : MonoBehaviour
{
    [Header("Props")]
    [SerializeField] private float _maximumFallSpeed;
    [SerializeField] private List<HighGroundSCB> _groundTypes;
    [Header("Components")]
    [SerializeField] private ParallaxEffect _parallaxEffect;
    [SerializeField] private Twins _playerOne, _playerTwo;


    private bool _initalJump;
    private Player_Actions _playerActions;
    public Twins _selectedPlayer = null;

    void Awake()
    {
        _playerActions = new();
    }

    void OnEnable()
    {
        _playerActions.Enable();
        _playerActions.Player.Jump.performed += JumpButton;
    }
    void OnDisable()
    {
        _playerActions.Player.Jump.performed -= JumpButton;
        _playerActions.Disable();
    }

    private void JumpButton(InputAction.CallbackContext context)
    {
        if (_initalJump) return;

        _initalJump = true;
        //do player jump things here
        int randomPlayer = Random.Range(0, 2);
        _selectedPlayer = randomPlayer == 0 ? _playerOne : _playerTwo;
        _selectedPlayer.Choose();
    }
    void Update()
    {
        if (!_initalJump) return;
        if (_selectedPlayer != null)
        {
            Debug.Log(_selectedPlayer);
            _parallaxEffect.SlideLayers(_selectedPlayer.Rb.linearVelocityY);
        }
    }
}

[Serializable]
public class Twins
{
    public bool Choosed;

    public BaseMovement Prefab;
    public Rigidbody2D Rb;
    [SerializeField] private Transform _parent;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _moveDelay;

    public void Choose()
    {
        Choosed = true;
        Prefab.enabled = true;
        Prefab.transform.SetParent(null);
        Rb.bodyType = RigidbodyType2D.Dynamic;
        Rb.AddForceY(_jumpForce, ForceMode2D.Impulse);
    }
    public void SetUnChoosed()
    {
        Prefab.enabled = false;
        Rb.bodyType = RigidbodyType2D.Kinematic;
        Rb.linearVelocity = Vector2.zero;
        Choosed = false;
        Prefab.transform.SetParent(_parent);
    }
}