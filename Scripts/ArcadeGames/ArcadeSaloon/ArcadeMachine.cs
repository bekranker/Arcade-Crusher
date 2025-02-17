using UnityEngine;
using UnityEngine.SceneManagement;
public class ArcadeMachine : MonoBehaviour, ISingleton<ArcadeMachine>
{
    [SerializeField] private string _sceneName;
    public void SceneChange(string name)
    {
        SceneManager.LoadScene(name);
    }
}