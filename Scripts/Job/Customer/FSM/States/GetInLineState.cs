
using UnityEngine;
public class GetInLineState : IState<CustomerBaseData<Customer>>
{
    private CustomerBaseData<Customer> _data;
    public void Init(CustomerBaseData<Customer> data)
    {
        _data = data;
    }

    public void OnEnter()
    {
        _data.MyCustomerHandler.Move(_data.Me);
    }

    public void OnExit()
    {
    }

    public void OnUpdate()
    {
    }
}