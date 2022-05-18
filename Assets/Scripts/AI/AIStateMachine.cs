using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateMachine : MonoBehaviour 
{
    private AIState _initialState;

    [HideInInspector]
    public AIState CurrentState;

    private bool _stateChanged;

    public void SetState(AIState state, bool force = false) 
    {
        if (CurrentState.GetType() != state.GetType() || force)
        {
            StateChange(state);
        }
    }

    public virtual void StateChange(AIState newState)
    {
        if (_stateChanged)
        {
            CurrentState.RecursiveExit();
        }
        else
        {
            _stateChanged = true;
        }
        CurrentState = newState;

    }

    private void InitCurrentState()
    {
        CurrentState.Parent = this;
        CurrentState.Complete = false;
        CurrentState.Enter();
    }

}
