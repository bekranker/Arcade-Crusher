using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using Random = UnityEngine.Random;
using UnityEngine.UI;
public class Dad : MonoBehaviour
{
    [Header("----Other Props")]
    [SerializeField] private Animator _animator;
    [Header("----Stan Props")]
    [SerializeField] private Slider _stanSlider;
    [SerializeField] private int _possibility;
    [SerializeField] private float _sleepTime;
    [SerializeField] private float _sleepTimeForSelectingAction;

    [Header("----Move Props")]
    [SerializeField] private float _speed;


    void Start()
    {
        InitializeDad();
    }
    public async void InitializeDad()
    {
        print("Prepeare Cooking");
        await UniTask.Delay(TimeSpan.FromSeconds(_sleepTimeForSelectingAction));
        SetAction();
    }
    private async void SetAction()
    {
        int selectAction = Random.Range(0, 100);
        if (selectAction <= _possibility)
        {
            _animator.Play("Sleep");
            await Stan();
        }
        else
        {
            _animator.Play("Cooking");
            await Waiting();
        }
        InitializeDad();
    }
    private async UniTask Stan()
    {
        float counter = _sleepTime;
        _stanSlider.maxValue = counter;
        while (counter > 0)
        {
            counter -= Time.deltaTime;
            _stanSlider.value = counter;
            await UniTask.Yield();
        }
    }
    private async UniTask Waiting()
    {
        float counter = _sleepTime;
        while (counter > 0)
        {
            counter -= Time.deltaTime;
            await UniTask.Yield();
        }
    }
}