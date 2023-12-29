using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentIdleState : AgentBaseState
{
    protected override AgentStateManager stateManager { get; set; }

    public AgentIdleState(AgentStateManager agentStateManager) : base(agentStateManager) { }
    

    public override void EnterState()
    {
        //stateManager.agentAnimation.SwitchAnimationState(AgentAnimation.Animations.Idle);
    }

    public override void UpdateState()
    {

    }

    public override void ExitState()
    {

    }
}
