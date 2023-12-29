using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentRenderer : MonoBehaviour
{
    [SerializeField] bool isFacingRight = true;
    private SpriteRenderer spriteRender;

    private void Awake()
    {
        spriteRender = GetComponent<SpriteRenderer>();
    }

    public void CheckIfFlip(bool isMovingRight)
    {
        if (isMovingRight != isFacingRight)
        {
            Flip();
        }
    }

    public void Flip()
    {
        Vector3 scaleToTurn = transform.localScale;
        scaleToTurn.x *= -1;
        transform.localScale = scaleToTurn;

        isFacingRight = !isFacingRight;
    }
}
