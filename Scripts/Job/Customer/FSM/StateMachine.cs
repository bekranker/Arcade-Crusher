/// <summary>
/// This class controls the Finite State Machine (FSM).
/// If a condition is met (while in any state), the current state can call ChangeState to transition to a new state and execute it.
/// </summary>
public class StateMachine<T> : IStateMachine<T> where T : IBaseData
{
    // Represents the current state of the state machine.
    private IState<T> _state;

    // Checks if there is any state currently active. Returns true if a state exists, otherwise false.
    private bool _hasAnyState => _state != null;

    /// <summary>
    /// Changes the current state to the specified state.
    /// If there is an active state, it calls OnExit to clean up the current state.
    /// Then, it initializes the new state, calls OnEnter to set it up, and transitions to the new state.
    /// </summary>
    /// <param name="state">The new state to transition to.</param>
    public void ChangeState(IState<T> state)
    {
        if (_hasAnyState)
            _state.OnExit();

        _state = state;
        _state.OnEnter();
    }

    /// <summary>
    /// Updates the current state every frame.
    /// If no state is active, it does nothing.
    /// Otherwise, it calls OnUpdate on the current state to execute its logic.
    /// </summary>
    public void UpdateStates()
    {
        if (!_hasAnyState)
            return;

        _state.OnUpdate();
    }
}