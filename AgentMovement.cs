using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentMovement : MonoBehaviour
{
    //  * improve upon input -> movement script interaction
    //  refactor code into smaller methods, standardize

    //  Optimize value updates

    //  figure out how to make a seperate agent attack script
    //      maybe add hault movement and more general methods that
    //      can be tapped into by external scripts like attack
    //          If this was done, would need to look over coyote

    //  Could make isGrounded method into to one returns bool in order
    //      to prevent invalid cached flags
    //  Could make isFalling method into to one returns bool in order
    //      to prevent invalid cached flags

    public enum MovementState { Simple, Air, Jump };
    [SerializeField] private MovementState currentMovementState;

    public MovementState CurrentMovementState
    {
        get { return currentMovementState; }
        private set
        {
            switch (value)
            {
                case MovementState.Simple:
                    currentMovementState = value;
                    startingAnimation = true;
                    break;

                case MovementState.Air:
                    currentMovementState = value;
                    startingAnimation = true;
                    break;

                case MovementState.Jump:
                    currentMovementState = value;
                    startingAnimation = true;
                    break;

                default:
                    Debug.LogError("Invalid movement state");
                    break;
            }
        }
    }

    [Header("State Stuff:")]
    [SerializeField] private bool isGrounded; //only for editor
    [SerializeField] private bool isFalling; //only for editor
    [SerializeField] private Vector2 movementInput;

    [Header("General Config:")]
    [SerializeField] [Range(0, 20)] private float jumpForce;
    [SerializeField] [Range(0, 1)] private float jumpCancelMultiplier;
    [SerializeField] [Range(0, 10)] private float moveSpeed;
    [SerializeField] [Range(1, 20)] private float accelerationRate;
    [SerializeField] [Range(1, 20)] private float decelerationRate;
    [SerializeField] [Range(0, 0.2f)] private float stoppingFriction;

    [Header("Advanced Config:")]
    [SerializeField] [Range(-1, 1)] private float groundCheckOffset;
    [SerializeField] [Range(0, 1)] private float groundCheckSize;
    [SerializeField] [Range(0, 2)] private float upGravityMultiplier;
    [SerializeField] [Range(0, 2)] private float downGravityMultiplier;
    [SerializeField] [Range(0, 0.2f)] private float speedSnapOffset;
    [SerializeField] [Range(0, 2)] private float stopPower;
    [SerializeField] [Range(0, 2)] private float turnPower;
    [SerializeField] [Range(0, 2)] private float forwardPower;
    [SerializeField] [Range(0, 2)] private float airAccelerationMultiplier;
    [SerializeField] [Range(0, 2)] private float airDecelerationMultiplier;

    [Header("Experimental Config:")]
    [SerializeField] private bool hasAdditiveJump;
    [SerializeField] private float yVelocityStateBuffer;
    [SerializeField] [Range(0, 0.2f)] private float coyoteTime;
    [SerializeField] private float lastGroundedTime;
    [SerializeField] private float lastJumpTime;
    [SerializeField] private float lastCancelTime;
    [SerializeField] private bool hasInertia;
    [SerializeField] [Range(0, 1)] private float inertiaStrength;
    [SerializeField] private bool hasStoppingBuffer;
    [SerializeField] [Range(0, 0.1f)] private float stoppingBufferWait;

    private bool startingAnimation;

    private float startingGravityScale;
    private float lastInput;

    private Rigidbody2D rigidBody2d;
    private AgentRenderer agentRenderer;
    private AgentAnimation agentAnimation;

    private void Awake()
    {
        rigidBody2d = GetComponent<Rigidbody2D>();
        agentRenderer = GetComponentInChildren<AgentRenderer>();
        agentAnimation = GetComponentInChildren<AgentAnimation>();
        startingGravityScale = rigidBody2d.gravityScale;
    }

    private void FixedUpdate()
    {
        UpdateLastTimes();

        ChangeMovementStates();
        HandleMovementStates();

        UpdateAnimations();
        ApplyWeighting();
        ApplyStoppingFriction();
        MoveHorizontally();
    }

    private void ChangeMovementStates()
    {
        if (CurrentMovementState == MovementState.Simple)
        {
            if (!GetIsGrounded()) 
            {
                CurrentMovementState = MovementState.Air;
                return;
            }

            if (CanJump())
            {
                Jump();

                CurrentMovementState = MovementState.Jump;
                return;
            }
        }
        else if (CurrentMovementState == MovementState.Air)
        {
            if (GetIsGrounded())
            {
                CurrentMovementState = MovementState.Simple;
                return;
            }

            if (CanJump())
            {
                Jump();

                CurrentMovementState = MovementState.Jump;
                return;
            }
        }
        else if (CurrentMovementState == MovementState.Jump)
        {
            if (GetIsGrounded())
            {
                CurrentMovementState = MovementState.Simple;
                return;
            }

            if (CanJumpCancel())
            {
                JumpCancel();
            }
        }
    }

    private void HandleMovementStates()
    {
        if (CurrentMovementState == MovementState.Simple)
        {
            // do something while state is active
        }
        else if (CurrentMovementState == MovementState.Air)
        {
            // do something while state is active
        }
        else if (CurrentMovementState == MovementState.Jump)
        {
            // do something while state is active
        }
    }

    private void UpdateAnimations()
    {
        if (CurrentMovementState == MovementState.Simple)
        {
            if (startingAnimation)
            {
                agentAnimation.CurrentAnimationState = AgentAnimation.AnimationState.base_idle;
                startingAnimation = false;
            }


            if (agentAnimation.CurrentAnimationState == AgentAnimation.AnimationState.base_idle)
            {
                if (Mathf.Abs(movementInput.x) > 0)
                {
                    agentAnimation.CurrentAnimationState = AgentAnimation.AnimationState.base_run;
                }
            }
            else if (agentAnimation.CurrentAnimationState == AgentAnimation.AnimationState.base_run)
            {
                if (Mathf.Abs(movementInput.x) == 0)
                {
                    agentAnimation.CurrentAnimationState = AgentAnimation.AnimationState.base_idle;
                }
            }
        }
        else if (CurrentMovementState == MovementState.Air || CurrentMovementState == MovementState.Jump)
        {
            if (startingAnimation)
            {
                agentAnimation.CurrentAnimationState = AgentAnimation.AnimationState.base_jump_up;
                startingAnimation = false;
            }


            if (agentAnimation.CurrentAnimationState == AgentAnimation.AnimationState.base_jump_up)
            {
                if (GetIsFalling())
                {
                    agentAnimation.CurrentAnimationState = AgentAnimation.AnimationState.base_jump_down;
                }
            }
            else if (agentAnimation.CurrentAnimationState == AgentAnimation.AnimationState.base_jump_down)
            {
                if (!GetIsFalling())
                {
                    agentAnimation.CurrentAnimationState = AgentAnimation.AnimationState.base_jump_up;
                }
            }
        }
    }

    #region Horizontal Movement
    public void OnHorizontalMovement(Vector2 movementInput)
    {
        if (hasStoppingBuffer)
        {
            if (movementInput.x == 0 && rigidBody2d.velocity.x != 0)
            {
                StartCoroutine(StoppingBuffer(movementInput));
                this.movementInput.x = lastInput;
            }
            else
            {
                lastInput = movementInput.x;
                this.movementInput.x = movementInput.x;
            }
        }
        else
        {
            lastInput = movementInput.x;
            this.movementInput.x = movementInput.x;
        }
    }

    private void MoveHorizontally()
    {
        float targetSpeed = movementInput.x * moveSpeed;
        float speedOffset = targetSpeed - rigidBody2d.velocity.x;

        float acceleration = FindAcceleration(targetSpeed);
        float power = FindPower(targetSpeed);

        float movement = Mathf.Pow(Mathf.Abs(speedOffset) * acceleration, power) * Mathf.Sign(speedOffset);
        if (hasInertia)
        {
            //  tends to overshoot target speed by a bit
            //  so needs to have a speedSnap for accuracy
            if (movement != 0)
            {
                movement = Mathf.Lerp(rigidBody2d.velocity.x, movement, 1 - inertiaStrength);
            }
        }

        if (Mathf.Abs(speedOffset) > speedSnapOffset)
        {
            rigidBody2d.AddForce(movement * Vector2.right);
        }
        else
        {
            rigidBody2d.velocity = new Vector2(targetSpeed, rigidBody2d.velocity.y);
        }

        if (movementInput.x != 0) { agentRenderer.CheckIfFlip(movementInput.x > 0); }
    }

    private IEnumerator StoppingBuffer(Vector2 movementInput)
    {
        yield return new WaitForSeconds(stoppingBufferWait);
        lastInput = movementInput.x;
    }

    private float FindAcceleration(float targetSpeed)
    {
        float acceleration;
        if (CurrentMovementState != MovementState.Simple) //
        {
            float airAccelerationRate = accelerationRate * airAccelerationMultiplier;
            float airDecelerationRate = decelerationRate * airDecelerationMultiplier;

            acceleration = Mathf.Abs(targetSpeed) > Mathf.Epsilon ? airAccelerationRate : airDecelerationRate;
        }
        else
        {
            acceleration = Mathf.Abs(targetSpeed) > Mathf.Epsilon ? accelerationRate : decelerationRate;
        }

        if (rigidBody2d.velocity.x > targetSpeed && targetSpeed > Mathf.Epsilon
             || rigidBody2d.velocity.x < targetSpeed && targetSpeed < -Mathf.Epsilon)
        {
            acceleration = 0;
        }

        return acceleration;
    }

    private float FindPower(float targetSpeed)
    {
        float power;
        if (Mathf.Abs(targetSpeed) < Mathf.Epsilon)
        {
            power = stopPower;
        }
        else if (Mathf.Abs(rigidBody2d.velocity.x) != 0 &&
                 Mathf.Sign(rigidBody2d.velocity.x) != Mathf.Sign(targetSpeed))
        {
            power = turnPower;
        }
        else
        {
            power = forwardPower;
        }

        return power;
    }

    #endregion Horizontal Movement

    #region Conditional Movement Modifiers
    private void ApplyStoppingFriction()
    {
        if (CurrentMovementState != MovementState.Simple) { return; }
        if (Mathf.Abs(movementInput.x) > Mathf.Epsilon) { return; }

        float frictionToAdd = Mathf.Min(Mathf.Abs(stoppingFriction), Mathf.Abs(rigidBody2d.velocity.x));

        frictionToAdd *= -1 * Mathf.Sign(rigidBody2d.velocity.x);
        rigidBody2d.AddForce(frictionToAdd * Vector2.right, ForceMode2D.Impulse);
    }

    private void ApplyWeighting()
    {
        if (CurrentMovementState == MovementState.Simple) { return; }

        if (GetIsFalling())
        {
            rigidBody2d.gravityScale = startingGravityScale * downGravityMultiplier;
        }
        else
        {
            rigidBody2d.gravityScale = startingGravityScale * upGravityMultiplier;
        }
    }
    #endregion Conditional Movement Modifiers

    #region Jumping
    private void Jump()
    {
        if (hasAdditiveJump)
        {
            //  - - - - - - - - - -  - needs testing !! - - - - - - - - - - - -
            //  is falling could cause issues (maybe out of sync with fixedUpdate
            //  physics) checking rigidbody2d directly here may prevent this 

            if (GetIsFalling())
            {
                rigidBody2d.AddForce(Mathf.Abs(rigidBody2d.velocity.y) * Vector2.up, ForceMode2D.Impulse);
            }

            rigidBody2d.AddForce(jumpForce * Vector2.up, ForceMode2D.Impulse);
        }
        else
        {
            if (jumpForce > rigidBody2d.velocity.y && jumpForce > 0
                 || jumpForce < rigidBody2d.velocity.y && jumpForce < 0)
            {
                rigidBody2d.velocity = new Vector2(rigidBody2d.velocity.x, jumpForce);
            }
        }

        lastGroundedTime = 0;
        lastJumpTime = 0;
    }

    public void OnJump()
    {
        lastCancelTime = 0;
        lastJumpTime = coyoteTime;
    }

    private bool CanJump()
    {
        return lastGroundedTime > 0 && lastJumpTime > 0;
    }


    private void JumpCancel()
    {
        float forceToAdd = -1 * rigidBody2d.velocity.y * jumpCancelMultiplier;
        rigidBody2d.AddForce(forceToAdd * Vector2.up, ForceMode2D.Impulse);

        lastCancelTime = 0;
    }

    public void OnJumpCancel()
    {
        lastCancelTime = coyoteTime;
    }

    private bool CanJumpCancel()
    {
        if (GetIsFalling()) { return false; }

        return lastCancelTime > 0;
    }
    #endregion Jumping

    #region State Updating
    private void UpdateLastTimes()
    {
        lastGroundedTime = Mathf.Clamp(lastGroundedTime -= Time.deltaTime, 0, lastGroundedTime);
        lastJumpTime = Mathf.Clamp(lastJumpTime -= Time.deltaTime, 0, lastJumpTime);
        lastCancelTime = Mathf.Clamp(lastCancelTime -= Time.deltaTime, 0, lastCancelTime);
    }

    public bool GetIsFalling()
    {
        isFalling = rigidBody2d.velocity.y < -yVelocityStateBuffer;

        return isFalling;
    }

    public bool GetIsGrounded()
    {
        Vector2 groundPoint = new Vector2(transform.position.x, transform.position.y + groundCheckOffset);
        Collider2D overlapResults = Physics2D.OverlapCircle(groundPoint, groundCheckSize, LayerMask.GetMask("Ground"));

        //  set to false if has positive y velocity
        if (rigidBody2d.velocity.y > yVelocityStateBuffer)
        {
            isGrounded = false;
        }
        else
        {
            isGrounded = overlapResults != null;
        }

        if (isGrounded) { lastGroundedTime = coyoteTime; }

        return isGrounded;
    }
    #endregion State Updating
}
