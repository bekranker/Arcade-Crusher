using UnityEngine;

public class PlateHandler : MonoBehaviour, IObjectInteractable
{
    [SerializeField] private WorkManager _workManager;
    public void ExecuteInteraction()
    {
        _workManager.TakePlate();
    }
}