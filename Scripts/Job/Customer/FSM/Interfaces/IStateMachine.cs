/// <summary>
/// Classes that inherit this interface will act as State Machine Handlers.
/// If a class implementing this interface is present in any object, that object will have a Finite State Machine (FSM).
/// </summary>
/// <typeparam name="T">The type of data associated with the state machine, constrained to IBaseData.</typeparam>
public interface IStateMachine : IBaseData
{
    /// <summary>
    /// Updates the topmost child state (i.e., the active state) every frame to execute its logic.
    /// </summary>
    void UpdateStates();

    /// <summary>
    /// Adds a new state to the state machine and transitions to it.
    /// </summary>
    /// <param name="state">The state to transition to.</param>
    void ChangeState(IState state);
}