using UnityEngine;
public class Money : MonoBehaviour, IObjectInteractable
{
    private MoneyHandler _moneyHandler;
    private int _tip;
    public void ExecuteInteraction()
    {
        _moneyHandler.IncreaseMoney(_tip);
    }
    public void Init(MoneyHandler moneyHandler, int tip)
    {
        _moneyHandler = moneyHandler;
        _tip = tip;
    }
}