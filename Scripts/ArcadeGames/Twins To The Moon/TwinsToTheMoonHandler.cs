using UnityEngine;
using UnityEngine.InputSystem;
using System;
using Random = UnityEngine.Random;
using DG.Tweening;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using Unity.Cinemachine;

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
/// * tahterevalliye (nasıl yazılıyor bilmiyom üşendim bakmaya) değince diğer oyuncuya geçecek ✓
/// * eğer geçiş yaparsa diğer oyuncunun zıplama animasyonu başlatılacak (Choosed & UnChooosed fonksiyonlarında yap ya da Animator de _currentHeight'ın değerine göre ayarlat) ✓
/// * shock wave'i bir kez daha dene yarım günden fazla alırsa genel polish zamanına bırak ✓
/// * prop spawna olasılık ekle ✓
/// 
/// ------------------------------------------ 12 Mayıs 2025 ------------------------------------------
/// * Ground Levellar bitince sonsuza gidecek şekilde randomize propları çağır. ✓
/// 
/// </summary>
public class TwinsToTheMoonHandler : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text _currentHeightTMP;

    [Header("Effects")]
    [SerializeField] private CinemachineImpulseSource _impulseSource;
    [SerializeField] private ParticleSystem _fallParticle;
    [SerializeField] private GameObject _fallBG;
    [Header("Shake Effect")]
    [SerializeField] private Transform _shakeTransform;
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
    public float _currentHeight;
    private bool _initalJump;
    private bool _falling;
    private Transform _currentParallaxParent;
    public Vector2 _bestHeight;
    private bool _swithchPlayer;
    private bool jumping;
    public event Action OnJump;
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
        if (!jumping)
        {
            HandleSpawning();
            HandleParallaxSpawn();
        }
        SwitchPlayer();
        if (!Jump)
        {
            if (_currentHeight >= _maximumFallSpeed)
            {
                if (_startPoint.position.y <= -_bestHeight.y)
                {
                    jumping = false;
                }
            }
            else
            {
                FallingPhase();
            }
            _currentHeight -= Time.deltaTime * _fallSpeed;
            _parallaxEffect.SlideLayers(_currentHeight);
        }
    }
    private void SwitchPlayer()
    {
        if (_startPoint.position.y >= 0)
        {
            jumping = true;
            if (_swithchPlayer)
            {
                SelectedPlayer.SetUnChoosed();
                DOTween.Kill(_shakeTransform);
                SelectedPlayer = SelectedPlayer == _playerOne ? _playerTwo : _playerOne;
                SelectedPlayer.Choose();
                Jump = true;
                StartCoroutine(JumpToTheBest());
                _swithchPlayer = false;
                _falling = false;
                _fallBG.SetActive(false);
                _fallParticle.Stop();
            }
        }
    }
    private IEnumerator JumpToTheBest()
    {
        Jump = true;
        Vector3 targetHeight = _bestHeight;
        _currentHeight = _jumpForce;
        while (-_startPoint.position.y < targetHeight.y)
        {
            _currentHeight++;
            _parallaxEffect.SlideLayers(_currentHeight);
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForEndOfFrame();
        Jump = false;
        _swithchPlayer = true;
        PushForce(_jumpForce);
    }
    void ChangeHeightTMP()
    {
        _currentHeightTMP.text = (-_startPoint.position.y).ToString("F2");
    }
    private void JumpButton(InputAction.CallbackContext context)
    {
        if (_initalJump) return;
        _swithchPlayer = true;
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
        jumping = false;
        PushForce(_jumpForce);
    }
    public void PushForce(float targetForce)
    {
        OnJump?.Invoke();
        StartCoroutine(PushForceIE(targetForce));
    }
    public IEnumerator PushForceIE(float targetForce = 0)
    {
        Jump = true;
        _currentHeight = 0;
        _impulseSource.GenerateImpulse();
        while (_currentHeight < targetForce)
        {
            _currentHeight++;
            _parallaxEffect.SlideLayers(_currentHeight);
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForEndOfFrame();
        _bestHeight = -_startPoint.position;
        Jump = false;
        jumping = false;
    }
    private void FallingPhase()
    {
        if (_falling) return;
        SelectedPlayer.Prefab.transform.DOMoveY(-2, 1f).SetEase(Ease.Linear);
        _shakeTransform.DOShakeRotation(.2f, _amount).SetLoops(-1);
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
        if (currentLevelLogic.Possibility < Random.Range(0, 100))
        {
            return null;
        }
        return _objectPool.Get(currentLevelLogic.Environments[Random.Range(0, currentLevel.Environments.Count - 1)]).GetComponent<TTMEnvironment>();
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
        Prefab.transform.SetParent(_parent);
        Prefab.transform.localPosition = _startPos.localPosition;
        Rb.linearVelocity = Vector2.zero;
        Rb.angularVelocity = 0;
        Rb.bodyType = RigidbodyType2D.Kinematic;
        Choosed = false;
        Prefab.enabled = false;
    }
}