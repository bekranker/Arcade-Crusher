// herhangi bir State oluşturmak istiyorsak bu interface miras alınmalıdır. 
// Oluşturulan State IBaseData'yı da miras almalıdır. T data generictir ve herhangi bir data aktarımı yapılabilir.
public interface IState : IBaseData
{
    void OnEnter();
    void OnExit();
    void OnUpdate();
}