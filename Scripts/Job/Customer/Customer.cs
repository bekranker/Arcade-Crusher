using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
public class Customer : MonoBehaviour
{
    [Header("---Movement")]
    [Tooltip("NPC movement Speed")]
    [SerializeField] private float _speed;
    [Header("---Prefab and Components")]
    [Tooltip("NPC's Canvas")]
    [SerializeField] private GameObject _orderBox;

    [Tooltip("NPC's Tooltip in the UI like X2 burger etc")]
    [SerializeField] private Order _orderUIPrefab;
    [SerializeField] private TMP_Text _NPCName;

    private SeatHandler _seatHandler;
    private Dictionary<FoodType, int> _myOrders;
    private CustomerType _customerType;
    private string _name;


    /// <summary>
    /// this funtion will be called when created.
    /// </summary>
    /// <param name="seatHandler"></param>
    public void Init(SeatHandler seatHandler, CustomerType typeOfCustomer)
    {
        _seatHandler = seatHandler;
        _myOrders = _seatHandler.GetOrderData();
        _customerType = typeOfCustomer;
        _name = _customerType.CustomerName;
        _NPCName.text = _name;
    }
    /// <summary>
    /// NPC movement function. The NPC will be moving towards seat position
    /// </summary>
    /// <param name="targetPoint"></param>
    public void Move(Vector3 targetPoint)
    {
        //Animation.SetBool(Walking);
        Movement(targetPoint);
    }
    /// <summary>
    /// the move loop with UniTask
    /// </summary>
    /// <param name="targetPoint">destination</param>
    private async void Movement(Vector3 targetPoint)
    {
        while (transform.position != targetPoint)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPoint, _speed * Time.deltaTime);
            await UniTask.Yield();
        }
        //after movement get the Tooltip box for oders
        Invoke(nameof(OrderMenu), .5f);

    }
    private void OrderMenu()
    {
        _orderBox.SetActive(true);

    }
    private void PrepeareOrder()
    {
        foreach (KeyValuePair<FoodType, int> orderItem in _myOrders)
        {
            Order tempOrderUIPrefab = Instantiate(_orderUIPrefab, _orderBox.transform);
            tempOrderUIPrefab.Init(orderItem.Key, orderItem.Value);
        }
    }
}