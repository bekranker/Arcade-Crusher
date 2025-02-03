using System.Collections.Generic;
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
    [SerializeField] private TMP_Text _NPCName;
    [SerializeField] private TMP_Text _stateName;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    public CustomerHandler MyCustomerHandler { get; set; }
    public CustomerType MyCustomerType { get; set; }
    private string _name;
    public IStateMachine MyStateMachine;
    public IdleState MyIdleState;
    public MoveState MoveState;
    public OrderState MyEatState;
    private WorkManager _workManager;
    public SeatHandler SeatHandler { get; set; }
    void Update()
    {
        MyStateMachine?.UpdateStates();
    }


    /// <summary>
    /// this funtion will be called when created.
    /// </summary>
    /// <param name="seatHandler"></param>
    public void Init(CustomerHandler customerHandler, CustomerType typeOfCustomer, WorkManager workManager, SeatHandler seatHandler)
    {
        _spriteRenderer.sprite = typeOfCustomer.PlaceHolder;
        MyCustomerHandler = customerHandler;
        MyCustomerType = typeOfCustomer;
        _name = MyCustomerType.CustomerName;
        _NPCName.text = _name;
        _workManager = workManager;
        SeatHandler = seatHandler;

        MyStateMachine = new StateMachine();
    }
    public void ChangeState(string stateName) => _stateName.text = stateName + " state";
}