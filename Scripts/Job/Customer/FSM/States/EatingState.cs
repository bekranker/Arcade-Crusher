
public class EatingState : IState<CustomerBaseData<Customer>>
{
    private CustomerBaseData<Customer> _data;
    public void Init(CustomerBaseData<Customer> data)
    {
        _data = data;
    }

    public void OnEnter()
    {
    }

    public void OnExit()
    {
    }

    public void OnUpdate()
    {
    }
}