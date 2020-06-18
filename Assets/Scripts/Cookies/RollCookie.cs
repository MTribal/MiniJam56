using My_Utils.Lean_Tween;
using UnityEngine;

public class RollCookie : Cookie
{
    public override void InitializeMachine()
    {
        SetState(new RollCookieStateWalk(this));
    }

    /// <summary>
    /// Called by walk animation.
    /// </summary>
    public void JumpMove(float duration)
    {
        //AudioManager.Instance.PlaySound("RollCookieStep");

        Vector2 moveOffset = MoveDirection * (moveSpeed / 10f);
        Vector2 targetPos = (Vector2)transform.position + moveOffset;
        LeanTween.Move(gameObject, targetPos, duration);
    }

    /// <summary>
    /// Called by walk animation.
    /// </summary>
    public void VerifyIfShouldAttack()
    {
        if (ShouldAttack()) Attack();
    }
}
