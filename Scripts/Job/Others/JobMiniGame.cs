using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Random = UnityEngine.Random;

public class JobMiniGame : MonoBehaviour
{
    public List<ARROW_KEYS> _keys = new();
    [SerializeField] private List<Image> _imageQueue = new();
    [SerializeField] private List<Vector3> _imageRotQueue = new();
    [SerializeField] private MoneyHandler _moneyHandler;
    [SerializeField] private float _timer;
    [SerializeField] private Slider _timeSlider;
    private int _index;
    private float _timerCount;
    private bool _timingStart;

    // Tuş eşleştirmeleri için Dictionary
    private Dictionary<ARROW_KEYS, KeyCode> _keyMappings = new Dictionary<ARROW_KEYS, KeyCode>()
    {
        { ARROW_KEYS.UP, KeyCode.UpArrow },
        { ARROW_KEYS.DOWN, KeyCode.DownArrow },
        { ARROW_KEYS.LEFT, KeyCode.LeftArrow },
        { ARROW_KEYS.RIGHT, KeyCode.RightArrow }
    };

    public void Init()
    {
        _index = 0;
        _timerCount = _timer;
        _timeSlider.value = _timer;
        _timeSlider.maxValue = _timer;
        _timingStart = true;
        SelectDirections();
    }

    private void Update()
    {
        Inputs();
        TimingStart();
    }

    private void SelectDirections()
    {
        ARROW_KEYS[] values = (ARROW_KEYS[])Enum.GetValues(typeof(ARROW_KEYS));
        for (int i = 0; i < 4; i++)
        {
            int index = Random.Range(0, values.Length);
            _keys.Add(values[index]);
            _imageQueue[i].transform.rotation = Quaternion.Euler(_imageRotQueue[index]);
            print(values[index]);
        }
    }
    private void TimingStart()
    {
        if (!_timingStart) return;
        if (_timerCount > 0)
        {
            _timerCount -= Time.unscaledDeltaTime;
        }
        else
        {
            Debug.Log("Wrong");
            _timingStart = false;
        }
        _timeSlider.value = _timerCount;
        return;

    }
    private void Inputs()
    {
        // Kazanma durumu
        if (_index >= _keys.Count)
        {
            print("Kazandınız!");
            return;
        }

        // Mevcut yönün karşılık gelen tuşunu kontrol et
        ARROW_KEYS currentKey = _keys[_index];
        if (Input.GetKeyDown(_keyMappings[currentKey]))
        {
            _index++;
            print("Doğru tuşa basıldı!");
        }
    }
}