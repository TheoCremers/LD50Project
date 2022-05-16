using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AIState : AIStateMachine 
{
    [HideInInspector]
    public BaseEnemyAI Entity; 

    public AIStateMachine Parent;

    public bool Complete;

    public virtual void Enter() { }

    public virtual void Logic() { }

    public virtual void Exit() { }

    public void RecursiveExit() 
    { 
        Exit();
        if (CurrentState != null)
        {
            CurrentState.RecursiveExit();
        }
    }

    public AIState GetDeepestState() 
    {
        if (CurrentState != null)
        {
            return CurrentState.GetDeepestState();
        }
        else
        {
            return this;
        }
    }
}
