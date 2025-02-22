using System.Collections;
using UnityEngine;

public class TimeHandler : MonoBehaviour
{
    // Singleton instance
    private static TimeHandler _instance;

    // Singleton erişim noktası
    public static TimeHandler Instance
    {
        get
        {
            if (_instance == null)
            {
                // Eğer instance yoksa, sahnede arar
                _instance = FindAnyObjectByType<TimeHandler>();

                // Sahnede de yoksa, yeni bir GameObject oluşturur
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject("TimeHandler");
                    _instance = singletonObject.AddComponent<TimeHandler>();
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        // Singleton yapısını koru
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject); // Başka bir instance varsa, bu objeyi yok et
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject); // Sahne geçişlerinde objeyi koru
        }
    }

    /// <summary>
    /// Freezing Time
    /// </summary>
    public void Freeze()
    {
        Time.timeScale = 0;
    }

    /// <summary>
    /// Freeze with given time
    /// </summary>
    /// <param name="delay"></param>
    public void Freeze(float delay)
    {
        StartCoroutine(DelayedFreeze(delay));
    }

    private IEnumerator DelayedFreeze(float delay)
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(delay);
        Time.timeScale = 1;
    }
}