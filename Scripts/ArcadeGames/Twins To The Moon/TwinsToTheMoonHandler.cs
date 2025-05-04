using UnityEngine;
using UnityEngine.InputSystem;
using System;
using Random = UnityEngine.Random;
using DG.Tweening;
using System.Collections.Generic;
using System.Collections;
using TMPro;

/// <summary>
/// 
/// ------------------------------------------ 28 Nisan 2025 ------------------------------------------
/// * ilk jumpi başlatabilir ✓
/// * anlık yüksekliği tutabilir ✓
/// * yavaşlatma ve hızlandırma efektlerini tutabilir ✓
/// * yavaşlatma ve hızlandırma efektlerini uygulayabilir ✓
/// * eğer sıçrayacağı yer ekranın boyutunu geçiyorsa efekt uygulanabilir ✓
/// * parallax efektinin hızını değiştirerek jump efektini buradan verebiliriz. ✓
/// 
/// * zıplama inputunu ilk aldığımızda oyunculardan birini random seçer ve onu parallaxın child objesi olmasından çıkarır ✓
///     - ayrıca zıplama animasyonunuda başlatır. ✓
///     - geri yere düştüğünde tekrar chil objesi olur ve oturma animasyonu oynar (eğer ki yanmadıysa) ✓
///         -- yanarsa ölme animasyonu oynar ve lose screen gelir ✓
/// 
/// ------------------------------------------ 5 Mayıs 2025 ------------------------------------------
/// * tahterevalliye (nasıl yazılıyor bilmiyom üşendim bakmaya) değince diğer oyuncuya geçecek
/// * eğer geçiş yaparsa diğer oyuncunun zıplama animasyonu başlatılacak (Choosed & UnChooosed fonksiyonlarında yap ya da Animator de _currentHeight'ın değerine göre ayarlat)
/// * shkock wave'i bir kez daha dene yarım günden fazla alırsa genel polish zamanına bırak
/// 
/// ------------------------------------------ 12 Mayıs 2025 ------------------------------------------
/// 
/// </summary>
public class TwinsToTheMoonHandler : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text _currentHeightTMP;

    [Header("Effects")]
    [SerializeField] private ParticleSystem _fallParticle;
    [SerializeField] private GameObject _fallBG;
    [Header("Shake Effect")]
    [SerializeField] private Transform _shakeTransform;
    [SerializeField] private Vector3 _frequency;
    [SerializeField] private float _amount;
    [Header("Props")]
    [SerializeField] private float _screenBounds;
    [SerializeField] private float _maximumFallSpeed;
    [SerializeField] private float _fallSpeed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private List<HighGroundSCB> _groundTypes;
    [SerializeField] private float _minDistanceBetweenObjects = 2f;
    [SerializeField] private float _spawnInterval = 5f; // Kaç birim yukarıda bir obje spawn edilecek
    [SerializeField] private float _parallaxSpawnInterval = 5f; // Kaç birim yukarıda bir obje spawn edilecek
    [Header("Components")]
    [SerializeField] private Transform _propsParent;
    [SerializeField] private PoolManager _objectPool;
    [SerializeField] private ParallaxEffect _parallaxEffect;
    [SerializeField] private Twins _playerOne, _playerTwo;
    [SerializeField] private PoolManager _poolManager;
    [SerializeField] private Transform _startPoint;
    [SerializeField] private LoseScreen _loseScreen;

    public float JumpForce { get => _jumpForce; set => _jumpForce = value; }
    public Twins SelectedPlayer { get; set; }
    public bool Jump { get; set; }
    private HashSet<int> _spawnedIndexes = new HashSet<int>();
    private HashSet<int> _parallaxSpawnedIndexes = new HashSet<int>();
    private List<Vector2> _spawnedPositions = new List<Vector2>();
    private Player_Actions _playerActions;
    private float _currentHeight;
    private bool _initalJump;
    private bool _falling;
    private Transform _currentParallaxParent;
    private float _bestHeight;
    private bool _swithchPlayer;
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
    void Update()
    {
        ChangeHeightTMP();
        if (!_initalJump) return;
        HandleSpawning();
        HandleParallaxSpawn();
        //SwitchPlayer();
        if (!Jump)
        {
            if (_currentHeight > _maximumFallSpeed)
            {
                if (_falling)
                {
                    _fallParticle.Stop();
                    _fallBG.SetActive(false);
                    _swithchPlayer = true;
                    _falling = false;
                }
                _currentHeight -= Time.deltaTime * _fallSpeed;

            }
            else
            {
                FallingPhase();
            }
            _parallaxEffect.SlideLayers(_currentHeight);
        }
    }
    // private void SwitchPlayer()
    // {
    //     if (!_swithchPlayer) return;
    //     if (SelectedPlayer.Prefab.transform.position.x > _screenBounds || SelectedPlayer.Prefab.transform.position.x < -_screenBounds) return;
    //     if (_startPoint.position.y < 1f && _startPoint.position.y > -1f)
    //     {
    //         MiniGameController.Instance.PauseTheGame();
    //         _currentHeight = 0;
    //         SelectedPlayer.Prefab.transform.position = Vector2.up * -5;
    //         DOTween.Kill(_shakeTransform);
    //         _shakeTransform.rotation = Quaternion.Euler(0, 0, 0);
    //         _fallParticle.Stop();
    //         _fallBG.SetActive(false);
    //         SelectedPlayer.SetUnChoosed();
    //         SelectedPlayer = (SelectedPlayer == _playerOne) ? _playerTwo : _playerOne;
    //         SelectedPlayer.Choose();
    //         PushForce(_bestHeight + _jumpForce);
    //         _swithchPlayer = false;
    //     }
    // }
    void ChangeHeightTMP()
    {
        _currentHeightTMP.text = (-_startPoint.position.y).ToString("F2");
    }
    private void JumpButton(InputAction.CallbackContext context)
    {
        if (_initalJump) return;

        _initalJump = true;
        // Choose a random player
        int randomPlayer = Random.Range(0, 2);
        SelectedPlayer = randomPlayer == 0 ? _playerOne : _playerTwo;

        // Call SetUnChoosed for the other player
        if (SelectedPlayer == _playerOne)
        {
            _playerTwo.SetUnChoosed();
        }
        else
        {
            _playerOne.SetUnChoosed();
        }

        // Perform jump actions for the selected player
        SelectedPlayer.Choose();
        PushForce(_jumpForce);
    }
    public void PushForce(float targetForce)
    {
        MiniGameController.Instance.ContunieToPlay();
        StartCoroutine(PushForceIE(targetForce));
    }
    public IEnumerator PushForceIE(float targetForce = 0)
    {
        Jump = true;
        _currentHeight = 0;
        while (_currentHeight < targetForce)
        {
            _currentHeight++;
            _parallaxEffect.SlideLayers(_currentHeight);
            yield return new WaitForEndOfFrame();
        }
        if (_bestHeight < -_startPoint.position.y)
            _bestHeight = -_startPoint.position.y;
        yield return new WaitForEndOfFrame();
        Jump = false;
    }
    private void FallingPhase()
    {
        if (_falling) return;
        SelectedPlayer.Prefab.transform.DOMoveY(-2, 1f).SetEase(Ease.Linear);
        _shakeTransform.DOPunchRotation(Vector3.forward * _amount, .1f).SetLoops(-1).SetEase(Ease.Linear);
        _fallBG.SetActive(true);
        _fallParticle.Play();
        _currentHeight = _maximumFallSpeed;
        _falling = true;
    }
    public void JumpingPhase()
    {

        _fallBG.SetActive(false);
        _fallParticle.Stop();
        _falling = false;
    }
    private void HandleParallaxSpawn()
    {
        int currentInterval = Mathf.FloorToInt(-_startPoint.position.y / _parallaxSpawnInterval);
        if (!_parallaxSpawnedIndexes.Contains(currentInterval))
        {
            _parallaxSpawnedIndexes.Add(currentInterval);
            float spawnY = currentInterval * _spawnInterval;
            GameObject spawnedParallax = SpawnParallaxObject();
            if (spawnedParallax == null) return;
            SpawnParallaxObjectAtHeight(spawnY + 5, spawnedParallax);
        }
    }
    private void HandleSpawning()
    {
        int currentInterval = Mathf.FloorToInt(-_startPoint.position.y / _spawnInterval);
        if (!_spawnedIndexes.Contains(currentInterval))
        {
            _spawnedIndexes.Add(currentInterval);
            float spawnY = currentInterval * _spawnInterval;
            //SpawnObjectAtHeight(spawnY, _boosterPrefab);
            SpawnObjectAtHeight(spawnY + 5, SpawnBooster());
            TTMEnvironment tTMEnvironment = SpawnRandomObject();
            if (tTMEnvironment == null) return;
            SpawnObjectAtHeight(spawnY + 5, tTMEnvironment);
        }
    }
    private HighGroundSCB CurrentLevel()
    {
        for (int i = 0; i < _groundTypes.Count; i++)
        {
            if (i == _groundTypes.Count - 1)
            {
                return _groundTypes[_groundTypes.Count - 1];
            }
            if (-_startPoint.position.y < _groundTypes[i].MaxHeight)
            {
                return _groundTypes[i];
            }
        }
        return null;
    }
    private GameObject SpawnParallaxObject()
    {
        HighGroundSCB currentLevel = CurrentLevel();
        if (currentLevel == null) return null;
        if (currentLevel.ParallaxObjects.Count == 0) return null;

        TTMEnvironmentParallaxLogic environmentParallaxLogic = currentLevel.ParallaxObjects[Random.Range(0, currentLevel.ParallaxObjects.Count)];

        string currentLevelParallax = environmentParallaxLogic.Environments[Random.Range(0, environmentParallaxLogic.Environments.Count)];
        GameObject parallaxObject = _objectPool.Get(currentLevelParallax);
        _currentParallaxParent = GameObject.Find(environmentParallaxLogic.Parent).transform;
        return parallaxObject;
    }
    private TTMEnvironment SpawnRandomObject()
    {
        HighGroundSCB currentLevel = CurrentLevel();
        if (currentLevel == null) return null;
        if (currentLevel.Environments.Count == 0) return null;


        TTMEnvironmentLogic currentLevelLogic = currentLevel.Environments[Random.Range(0, currentLevel.Environments.Count)];
        return _objectPool.Get(currentLevelLogic.Environments[Random.Range(0, currentLevel.Environments.Count)]).GetComponent<TTMEnvironment>();
    }
    private TTMEnvironment SpawnBooster() => _objectPool.Get("booster").GetComponent<TTMEnvironment>();

    private void SpawnParallaxObjectAtHeight(float height, GameObject objectToSpawn)
    {
        float xPosition = Random.Range(-_screenBounds, _screenBounds); // sahne genişliğine göre ayarla
        Vector3 spawnPosition = new Vector3(xPosition, height, 0f);
        objectToSpawn.transform.SetParent(_currentParallaxParent);
        objectToSpawn.transform.localPosition = spawnPosition;
    }
    private void SpawnObjectAtHeight(float height, TTMEnvironment poolObject)
    {
        Vector3 spawnPosition;
        int attempts = 0;
        const int maxAttempts = 20;
        do
        {
            float xPosition = Random.Range(-_screenBounds, _screenBounds);
            spawnPosition = new Vector3(xPosition, height, 0f);
            attempts++;
        }
        while (!IsPositionValid(spawnPosition) && attempts < maxAttempts);

        if (attempts >= maxAttempts)
        {
            Debug.LogWarning("Uygun pozisyon bulunamadı, spawn iptal edildi.");
        }

        poolObject.Initialize(this, _loseScreen, _poolManager);
        poolObject.transform.SetParent(_propsParent);
        poolObject.transform.localPosition = spawnPosition;
        _spawnedPositions.Add(spawnPosition);
    }
    private bool IsPositionValid(Vector3 newPosition)
    {
        foreach (var pos in _spawnedPositions)
        {
            if (Vector2.Distance(newPosition, pos) < _minDistanceBetweenObjects)
                return false;
        }
        return true;
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
    [SerializeField] private Transform _startPos;
    [SerializeField, Range(-10, 0)] private float _initalHeight;
    public void Choose()
    {
        DOTween.Kill(Prefab.transform);
        Choosed = true;
        Prefab.enabled = true;
        Prefab.transform.SetParent(null);
        Rb.bodyType = RigidbodyType2D.Dynamic;
        Prefab.transform.DOMoveY(_initalHeight, _moveDelay).SetEase(Ease.OutQuad).SetUpdate(true);
    }
    public void SetUnChoosed()
    {
        DOTween.Kill(Prefab.transform);
        Rb.linearVelocity = Vector2.zero;
        Rb.angularVelocity = 0;
        Rb.bodyType = RigidbodyType2D.Kinematic;
        Choosed = false;
        Prefab.transform.SetParent(_parent);
        Prefab.transform.localPosition = _startPos.localPosition;
        Prefab.enabled = false;
    }
}