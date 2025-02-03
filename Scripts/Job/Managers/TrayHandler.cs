using UnityEngine;

public class TrayHandler : MonoBehaviour
{
    [SerializeField] private WorkManager _workManager;
    void OnEnable()
    {
        WorkManager.HandAction += ChangePos;
    }
    void OnDisable()
    {
        WorkManager.HandAction -= ChangePos;
    }

    public void ChangePos()
    {
        float distanceEach = .5f / _workManager.HandGameObjects.Count;
        for (int i = 0; i < _workManager.HandGameObjects.Count; i++)
        {
            _workManager.HandGameObjects[i].transform.localPosition = new Vector2((i != 0) ? distanceEach + _workManager.HandGameObjects[i - 1].transform.localPosition.x : -.25f + distanceEach, .25f);
        }
    }

}
