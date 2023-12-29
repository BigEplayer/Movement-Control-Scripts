using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentAnimation : MonoBehaviour
{
    public enum AnimationState
    {
        base_idle,
        base_run,
        base_jump_up,
        base_jump_down
    }

    [SerializeField] private AnimationState currentAnimationState;

    public AnimationState CurrentAnimationState
    {
        get { return currentAnimationState; }

        set
        {
            if (value == currentAnimationState) { return; }

            currentAnimationState = value;
            animator.Play(value.ToString());
        }
    }

    private Animator animator;


    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
}