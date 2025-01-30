using UnityEngine;

public class TrashHandler : MonoBehaviour, IObjectInteractable
{
    [SerializeField] private WorkManager _workManager;

    public void ExecuteInteraction()
    {
        _workManager.ThrowToTrash();
    }
}
