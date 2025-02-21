using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using TMPro;
using ZilyanusLib.Audio;
using System.Collections;

//A
public class SimonSaidHandler : MonoBehaviour
{
    [SerializeField] private Color _listenPressedColor;
    [SerializeField] private List<SimonsButton> _buttons;
    [SerializeField] private float _delay;
    [SerializeField, Range(0, .15f)] private float _delayMighnus;
    [SerializeField] private float _soundVolume;
    [SerializeField] private Sprite _unPressedSprite;
    [SerializeField] private Sprite _pressedSprite;
    [SerializeField] private int _startCount;
    [Header("---Components")]
    [SerializeField] private SimonSaidHealthHandler _simonSaidHealthHandler;
    [SerializeField] private SimonsSaid_ButtonEffect _simonsSaid_ButtonEffect;
    public static event Action<Vector3> OnTrueInput;
    public static event Action OnWrongInput, OnListen;
    private Queue<SimonsButton> _buttonQueue = new();
    private Queue<SimonsButton> _inputQueue = new();
    private Queue<SimonsButton> _initialQueue = new();
    private int _count;
    private bool _canListenInputs = false;
    private SimonsButton _currentButton;
    private float _delayCounter;


    void Start()
    {
        Init();
    }
    public void Init()
    {
        foreach (SimonsButton button in _buttons)
        {
            button.Init();
        }
        _delayCounter = _delay;
        _count = _startCount;
        StartCoroutine(CreateQueue(_startCount));
    }

    public IEnumerator CreateQueue(int count)
    {
        _inputQueue.Clear();
        _buttonQueue.Clear();
        _canListenInputs = false;
        yield return new WaitForSeconds(.5f);
        for (int i = 0; i < count; i++)
        {
            int selectRandomButton = Random.Range(0, _buttons.Count);
            _buttons[selectRandomButton].ChangeSprite(_unPressedSprite);
            _initialQueue.Enqueue(_buttons[selectRandomButton]);
        }
        foreach (var button in _initialQueue)
        {
            _buttonQueue.Enqueue(button);
            _inputQueue.Enqueue(button);
        }

        _currentButton = _inputQueue.Dequeue();
        StartCoroutine(PlayQueue());
    }
    void Update()
    {
        ListenInputs();
    }
    private void ListenInputs()
    {
        if (!_canListenInputs) return;
        Debug.Log("girdi 1");

        if (Input.GetKeyDown(_currentButton.ButtonKeyCode))
        {
            _currentButton.PressMe(.5f, _pressedSprite);
            OnTrueInput?.Invoke(_currentButton.ButtonPosition);
            Debug.Log("True Input");
            return;
        }
        if (Input.GetKeyUp(_currentButton.ButtonKeyCode))
        {
            _currentButton.ReleaseMe(_unPressedSprite);
            if (_inputQueue.Count == 0)
            {
                _count++;
                _delayCounter -= _delay;
                StartCoroutine(CreateQueue(1));
                return;
            }
            _currentButton = _inputQueue.Dequeue();
            return;
        }
    }
    private IEnumerator PlayQueue()
    {
        for (int i = 0; i < _count; i++)
        {
            SimonsButton simonsButton = _buttonQueue.Dequeue();
            simonsButton.PlayMe(_soundVolume);
            yield return new WaitForSeconds(_delay / 2);
            OnListen?.Invoke();
            simonsButton.ChangeColor(_listenPressedColor);
            yield return new WaitForSeconds(_delay / 2);
            simonsButton.ChangeColor(simonsButton.UnPressedColor);
        }
        _canListenInputs = true;
    }
}
[Serializable]
public class SimonsButton
{
    public TMP_Text ButtonText;
    public SpriteRenderer ButtonSpriteRenderer;
    public string ButtonName;
    public AudioClip ButtonAudioClip;
    public Color PressedColor, UnPressedColor;
    private string _path = "MiniGames/SimonSaid/";
    public KeyCode ButtonKeyCode;
    public Vector3 ButtonPosition { get; set; }
    public void Init()
    {
        ButtonText.text = ButtonName;
        ButtonPosition = ButtonSpriteRenderer.gameObject.transform.position;
    }
    public void PlayMe(float volume)
    {
        AudioClass.PlayAudio($"{_path + ButtonName}", volume);
    }
    public void ReleaseMe(Sprite sprite)
    {
        ChangeSprite(sprite);
        ChangeColor(UnPressedColor);
    }
    public void PressMe(float volume, Sprite sprite)
    {
        PlayMe(volume);
        ChangeSprite(sprite);
        ChangeColor(PressedColor);
    }
    public void ChangeSprite(Sprite sprite)
    {
        ButtonSpriteRenderer.sprite = sprite;
    }
    public void ChangeColor(Color color)
    {
        ButtonSpriteRenderer.color = color;
    }
}