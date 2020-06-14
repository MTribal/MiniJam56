using My_Utils.Lean_Tween;
using UnityEngine;

public class StuffedCookie : Cookie
{
    /// <summary>
    /// Called by walk animation.
    /// </summary>
    public void JumpMove(float duration)
    {
        Vector2 moveOffset = MoveDirection * (moveSpeed / 10f);
        Vector2 targetPos = (Vector2)transform.position + moveOffset;
        LeanTween.Move(gameObject, targetPos, duration);
    }
}
