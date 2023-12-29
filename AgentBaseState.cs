using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AgentBaseState
{
    protected abstract AgentStateManager stateManager { get; set; }

    public AgentBaseState(AgentStateManager agentStateManager)
    {
        stateManager = agentStateManager;
    }

    public abstract void EnterState();

    public abstract void UpdateState();

    public abstract void ExitState();
}
