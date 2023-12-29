using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInput : MonoBehaviour
{
    [field: SerializeField] public UnityEvent<Vector2> onPlayerMovementKeyPressed { get; set; }
    [field: SerializeField] public UnityEvent onPlayerJumpKeyPressed { get; set; }
    [field: SerializeField] public UnityEvent onPlayerJumpKeyReleased { get; set; }
    [field: SerializeField] public UnityEvent onPlayerAttackKeyPressed { get; set; }
    [field: SerializeField] public UnityEvent onPlayerAttackKeyReleased { get; set; }

    [field: SerializeField] public UnityEvent<Vector2> onPointerPositionChanged { get; set; }

    private void Update()
    {
        GetJumpInput();
        GetAttackInput();
        GetPointerInput();
    }

    private void FixedUpdate()
    {
        GetMovementInput();
    }

    private void GetMovementInput()
    {
        int xInput = (int)Input.GetAxisRaw("Horizontal");
        int yInput = (int)Input.GetAxisRaw("Vertical");

        onPlayerMovementKeyPressed?.Invoke(new Vector2(xInput, yInput));
    }

    private void GetJumpInput()
    {
        if (Input.GetKeyDown(KeyCode.Space)) //maybe add y input here
        {
            onPlayerJumpKeyPressed?.Invoke();
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            onPlayerJumpKeyReleased?.Invoke();
        }
    }

    private void GetAttackInput()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            onPlayerAttackKeyPressed?.Invoke();
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            onPlayerAttackKeyReleased?.Invoke();
        }
    }

    private void GetPointerInput()
    {
        Vector2 pointerPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        onPointerPositionChanged?.Invoke(pointerPosition);
    }
}
