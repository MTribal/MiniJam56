using My_Utils.Audio;
using My_Utils.Lean_Tween;
using UnityEngine;

public class StuffedCookie : Cookie
{
    /// <summary>
    /// Called by walk animation.
    /// </summary>
    public void JumpMove(float duration)
    {
        if (!GameManager.Instance.IsPlaying) return;
        Vector2 moveOffset = MoveDirection * (moveSpeed / 10f);
        Vector2 targetPos = (Vector2)transform.position + moveOffset;
        LeanTween.Move(gameObject, targetPos, duration);
    }

    public override void Attack()
    {
        base.Attack();
        AudioManager.Instance.PlaySound("StuffedCookieAttack");
    }


    /// <summary>
    /// Called by walk animation.
    /// </summary>
    public void PlayStepAudio()
    {
        //AudioManager.Instance.PlaySound("StuffedCookieStep");
    }
}
