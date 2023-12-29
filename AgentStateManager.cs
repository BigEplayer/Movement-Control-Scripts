using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentStateManager : MonoBehaviour
{
    AgentBaseState currentState;

    private AgentStateCreator stateCreator;
    private AgentAnimation agentAnimation;

    private void Awake()
    {
        stateCreator = new AgentStateCreator(this);
        agentAnimation = GetComponentInChildren<AgentAnimation>();
    }

    private void Start()
    {
        currentState = stateCreator.CreateIdle();
        currentState.EnterState();
    }

    private void Update()
    {
        currentState.UpdateState();
    }

    public void SwitchState(AgentBaseState state)
    {
        currentState = state;
        currentState.EnterState();
    }
}
