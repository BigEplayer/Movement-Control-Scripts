using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentStateCreator
{
    AgentStateManager stateManager;

    public AgentStateCreator(AgentStateManager agentStateManager)
    {
        stateManager = agentStateManager;
    }

    public AgentBaseState CreateIdle()
    {
        return new AgentIdleState(stateManager);
    }
}
