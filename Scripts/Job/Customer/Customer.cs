using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
public class Customer : MonoBehaviour, IBaseData
{
    [Header("---Movement")]

    [Tooltip("NPC movement Speed")]
    [SerializeField] private float _speed;
    [Header("---Prefab and Components")]
    public Animator MyAnimator;
    [Tooltip("NPC's Canvas")]
    [SerializeField] private GameObject _orderBox;

    [Tooltip("NPC's Tooltip in the UI like X2 burger etc")]
    [SerializeField] private Order _orderUIPrefab;
    [SerializeField] private TMP_Text _NPCName;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    public CustomerHandler MyCustomerHandler { get; set; }
    private Dictionary<FoodType, int> _myOrders;
    public CustomerType MyCustomerType { get; set; }
    private string _name;
    public IStateMachine<CustomerBaseData<Customer>> MyStateMachine;
    public IdleState MyIdleState = new();
    public GetInLineState MyGetInLineState = new();
    public EatingState MyEatState = new();
    void Update()
    {
        MyStateMachine?.UpdateStates();
    }


    /// <summary>
    /// this funtion will be called when created.
    /// </summary>
    /// <param name="seatHandler"></param>
    public void Init(CustomerHandler customerHandler, CustomerType typeOfCustomer)
    {
        _spriteRenderer.sprite = typeOfCustomer.PlaceHolder;
        MyCustomerHandler = customerHandler;
        _myOrders = MyCustomerHandler.GetOrderData();
        MyCustomerType = typeOfCustomer;
        _name = MyCustomerType.CustomerName;
        _NPCName.text = _name;

        CustomerBaseData<Customer> customerBaseData = new(MyCustomerType.CustomerName, MyAnimator, _orderUIPrefab, _orderBox, _speed, this, MyCustomerHandler);
        MyEatState.Init(customerBaseData);
        MyGetInLineState.Init(customerBaseData);
        MyIdleState.Init(customerBaseData);

        MyStateMachine = new StateMachine<CustomerBaseData<Customer>>();
    }
    /// <summary>
    /// NPC movement function. The NPC will be moving towards seat position
    /// </summary>
    /// <param name="targetPoint"></param>
    public void Move(Vector3 targetPoint)
    {
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
        MyStateMachine.ChangeState(MyIdleState);
    }
    public void OrderMenu()
    {
        _orderBox.SetActive(true);
        PrepeareOrder();
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
public class CustomerBaseData<T> : IBaseData
{
    public string Name;
    public Animator AnimatorComponent;
    public Order MyOrderUIPrefab;
    public GameObject MyOrderBox;
    public float Speed;
    public Customer Me;
    public CustomerHandler MyCustomerHandler;

    public CustomerBaseData(string name, Animator animator, Order myOrderPrefabUI, GameObject orderBox, float speed, Customer customer, CustomerHandler customerHandler)
    {
        Name = name;
        AnimatorComponent = animator;
        MyOrderUIPrefab = myOrderPrefabUI;
        MyOrderBox = orderBox;
        Speed = speed;
        Me = customer;
        MyCustomerHandler = customerHandler;
    }
}