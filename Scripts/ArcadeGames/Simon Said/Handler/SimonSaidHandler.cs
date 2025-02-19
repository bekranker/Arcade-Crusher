using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using TMPro;
using ZilyanusLib.Audio;
public class SimonSaidHandler : MonoBehaviour
{
    [SerializeField] private List<SimonsButton> _buttons;
    private Queue<SimonsButton> _buttonQueue = new();
    private int _count;



    public void Init()
    {
        foreach (SimonsButton button in _buttons)
        {
            button.ButtonText.text = button.ButtonName;
        }
        _count = 2;
        CreateQueue();
    }
    public void CreateQueue()
    {
        for (int i = 0; i < _count; i++)
        {
            int selectRandomButton = Random.Range(0, _buttons.Count);
            _buttonQueue.Enqueue(_buttons[selectRandomButton]);
        }
    }
    private void PlayQueue()
    {
        for (int i = 0; i < _count; i++)
        {

        }
    }
}
[Serializable]
public class SimonsButton
{
    public TMP_Text ButtonText;
    public SpriteRenderer ButtonSpriteRenderer;
    public string ButtonName;
    public AudioClip ButtonAudioClip;
}