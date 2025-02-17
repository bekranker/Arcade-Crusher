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

    [SerializeField] private List<Transform> _stants = new();

    void Start()
    {
        Move();
    }
    public async void Move()
    {
        int tempRandStand = Random.Range(0, _stants.Count);
        Vector3 targetStand = _stants[tempRandStand].transform.position;
        _animator.Play("Move");
        while (transform != null && transform.position.x != targetStand.x)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetStand, _speed * Time.deltaTime);
            await UniTask.Yield();
        }
        transform.position = targetStand;

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
        Move();
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
        counter = 0;
    }
    private async UniTask Waiting()
    {
        float counter = _sleepTime;
        while (counter > 0)
        {
            counter -= Time.deltaTime;
            await UniTask.Yield();
        }
        counter = 0;
    }
}