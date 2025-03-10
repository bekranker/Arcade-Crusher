using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;


public class LevelTransaction : MonoBehaviour, ISingleton<LevelTransaction>
{
    public static LevelTransaction Instance;
    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
            Destroy(gameObject);
    }
    [SerializeField, Range(0, 3)] private float _delayMilliseconds;
    public async void GoThatScene(string sceneName)
    {
        print("Transacting level");
        await UniTask.Delay(System.TimeSpan.FromSeconds(_delayMilliseconds));
        SceneManager.LoadScene(sceneName);
    }
}