using System.Collections.Generic;
using UnityEngine;

public class PropFinder : MonoBehaviour
{
    [Header("-----Components")]
    [SerializeField] private PoolManager _poolManager;
    [SerializeField] private Camera _cam;
    [Header("-----Props")]
    public List<PropFinderLogic> SignedObjects = new();
    [SerializeField] private RectTransform _parentCanvas;
    public void AddToList(Transform objectT)
    {
        CheckListAndRemove(objectT);
        PropFinderLogic propFinderLogic = new();
        propFinderLogic.InitPropFinderLogic(objectT, _poolManager.Get("Navigator"), _parentCanvas);
        SignedObjects.Add(propFinderLogic);
    }
    private void CheckListAndRemove(Transform objectT)
    {
        for (int i = 0; i < SignedObjects.Count; i++)
        {
            if (SignedObjects[i].SceneProp == objectT)
            {
                SignedObjects.RemoveAt(i);
                break;
            }
        }
    }
    void Update()
    {
        Execute();
    }
    void Execute()
    {
        foreach (PropFinderLogic obj in SignedObjects)
        {
            if (obj != null && obj.SceneProp != null)
            {
                if (OffTheScreen(obj.SceneProp))
                {
                    obj.Navigator.transform.position = _cam.WorldToScreenPoint(obj.SceneProp.position);
                    print("OnScreen");
                }
                else
                {
                    _poolManager.Return(obj.Navigator);
                }
            }
        }
    }
    private bool OffTheScreen(Transform objectT)
    {
        Vector3 rightCorner = _cam.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        return objectT.position.x > rightCorner.x || objectT.position.x < -rightCorner.x ||
               objectT.position.y > rightCorner.y || objectT.position.y < -rightCorner.y;
    }
}
public class PropFinderLogic
{
    public Transform SceneProp;
    public GameObject Navigator;
    public void InitPropFinderLogic(Transform sceneProp, GameObject navigator, RectTransform _parentCanvas)
    {
        SceneProp = sceneProp;
        Navigator = navigator;
        navigator.transform.SetParent(_parentCanvas.transform);
        navigator.transform.localPosition = Vector3.zero;
        navigator.transform.localScale = Vector3.one;
    }
}