using UnityEngine;
public class ArcadeMachine : MonoBehaviour, ISingleton<ArcadeMachine>, IObjectInteractable
{
    [SerializeField] private string _sceneName;

    public ArcadeMachine Instance { get; set; }

    public void ExecuteInteraction()
    {
        SceneChange();
    }

    public void SceneChange()
    {
        print("Changing Scene");
        LevelTransaction.Instance.GoThatScene(_sceneName);
    }
}