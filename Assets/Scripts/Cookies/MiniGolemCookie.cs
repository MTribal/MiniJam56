using My_Utils.Lean_Tween;
using UnityEngine;

public class MiniGolemCookie : Cookie
{
    private bool _splitingRank;

    public void SetSplitingRank(bool value)
    {
        _splitingRank = value;
    }

    /// <summary>
    /// Called by walk animation.
    /// </summary>
    public void JumpMove(float duration)
    {
        if (_splitingRank) return;

        Vector2 moveOffset = MoveDirection * (moveSpeed / 10f);
        Vector2 targetPos = (Vector2)transform.position + moveOffset;
        LeanTween.Move(gameObject, targetPos, duration);
    }
}
