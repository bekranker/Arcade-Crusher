using UnityEngine;
using UnityEngine.InputSystem;
using System;
using Random = UnityEngine.Random;
using DG.Tweening;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using Unity.Cinemachine;


public class TwinsToTheMoonHandler : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text _currentHeightTMP;

    [Header("Effects")]
    [SerializeField] private CinemachineImpulseSource _impulseSource;
    [SerializeField] private Transform _cameraT;
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
    [SerializeField] private GeneralHearthManager _generalHearthManager;
    public float JumpForce { get => _jumpForce; set => _jumpForce = value; }
    public Twins SelectedPlayer { get; set; }
    public bool Jump { get; set; }
    private Player_Actions _playerActions;
    public float _currentHeight;
    private bool _initalJump;
    private bool _falling;
    private Transform _currentParallaxParent;
    public Vector2 _bestHeight;
    private bool _swithchPlayer;
    public event Action OnJump;
    void Awake()
    {
        _playerActions = new();
    }
    void OnEnable()
    {
        _playerActions.Enable();
        _playerActions.Player.Jump.performed += JumpButton;
        _generalHearthManager.OnHit += DamageEffect;
    }
    void OnDisable()
    {
        _playerActions.Player.Jump.performed -= JumpButton;
        _generalHearthManager.OnHit -= DamageEffect;
        _playerActions.Disable();
    }
    void Update()
    {
        ChangeHeightTMP();
        if (!_initalJump) return;
        SwitchPlayer();
        if (!Jump)
        {
            if (_currentHeight < _maximumFallSpeed)
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
            float targetDirection = SelectedPlayer.CorretPosDirection;
            if (targetDirection < 0)
            {
                if (SelectedPlayer.Prefab.transform.position.x > 0)
                {
                    _loseScreen.LoseGame();
                    return;
                }
            }
            else
            {
                if (SelectedPlayer.Prefab.transform.position.x < 0)
                {
                    _loseScreen.LoseGame();
                    return;
                }
            }

            if (_swithchPlayer)
            {
                _cameraT.rotation = Quaternion.Euler(0, 0, 0);
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
        PushForce(_jumpForce);
    }
    public void PushForce(float targetForce)
    {
        OnJump?.Invoke();
        StartCoroutine(PushForceIE(targetForce));
        HandleParallaxSpawn();
        HandleSpawning();
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
    public void HandleParallaxSpawn()
    {
        int possibility = Random.Range(0, 100);
        if (possibility <= 50) return;
        GameObject spawnedParallax = SpawnParallaxObject();
        if (spawnedParallax == null) return;
        SpawnParallaxObjectAtHeight(5, spawnedParallax);
    }
    private Vector3 _previousSpawnPosition;
    public void HandleSpawning()
    {
        _previousSpawnPosition = SpawnObjectAtHeight(4, SpawnBooster());
        TTMEnvironment tTMEnvironment = SpawnRandomObject();
        if (tTMEnvironment == null) return;
        SpawnObjectAtHeight(5, tTMEnvironment);
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
    private Vector2 _randomPos;
    private GameObject SpawnParallaxObject()
    {
        HighGroundSCB currentLevel = CurrentLevel();
        if (currentLevel == null) return null;
        if (currentLevel.ParallaxObjects.Count == 0) return null;

        TTMEnvironmentParallaxLogic environmentParallaxLogic = currentLevel.ParallaxObjects[Random.Range(0, currentLevel.ParallaxObjects.Count)];
        if (environmentParallaxLogic.SpawnPoses.Count != 0)
            _randomPos = environmentParallaxLogic.SpawnPoses[Random.Range(0, environmentParallaxLogic.SpawnPoses.Count)];
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
    private void DamageEffect()
    {
        StartCoroutine(SelectedPlayer.DamageEffectIE());
    }
    private void SpawnParallaxObjectAtHeight(float height, GameObject objectToSpawn)
    {
        float xPosition = Random.Range(-_screenBounds, _screenBounds);
        Vector3 spawnPosition = new Vector3(xPosition, height, 0f);
        if (_randomPos.x != 0) spawnPosition.x = _randomPos.x;
        objectToSpawn.transform.position = spawnPosition;
        objectToSpawn.transform.SetParent(_currentParallaxParent);
    }
    private Vector3 SpawnObjectAtHeight(float height, TTMEnvironment poolObject)
    {
        poolObject.Initialize(this, _loseScreen, _poolManager);
        float xPosition = Random.Range(-_screenBounds, _screenBounds); // sahne genişliğine göre ayarla
        Vector3 spawnPosition = new Vector3(xPosition, height, 0f);
        if (Mathf.Abs(spawnPosition.x - _previousSpawnPosition.x) <= _minDistanceBetweenObjects)
        {
            if (spawnPosition.x + _minDistanceBetweenObjects < _screenBounds)
                spawnPosition.x += _minDistanceBetweenObjects;
            else if (spawnPosition.x - _minDistanceBetweenObjects > -_screenBounds)
                spawnPosition.x -= _minDistanceBetweenObjects;
        }
        poolObject.transform.position = spawnPosition;
        poolObject.transform.SetParent(_propsParent);
        return spawnPosition;
    }
}

[Serializable]
public class Twins
{
    public bool Choosed;
    public BaseMovement Prefab;
    public Rigidbody2D Rb;
    public float CorretPosDirection;
    [SerializeField] private Transform _parent;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _moveDelay;
    [SerializeField] private Transform _startPos;
    [SerializeField, Range(-10, 0)] private float _initalHeight;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Sprite _damagedSprite;
    [SerializeField] private Sprite _normalSprite;
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
    public IEnumerator DamageEffectIE()
    {
        _spriteRenderer.sprite = _damagedSprite;
        yield return new WaitForSecondsRealtime(1f);
        _spriteRenderer.sprite = _normalSprite;
    }
}