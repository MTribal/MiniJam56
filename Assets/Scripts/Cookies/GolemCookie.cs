using My_Utils.Audio;
using My_Utils.Lean_Tween;
using UnityEngine;

public class GolemCookie : Cookie
{
    [SerializeField] private float _splitMiniGolemsDur = default;
    [SerializeField] private LeanTweenType _splitGolemsEase = default;
    [SerializeField] private MiniGolemCookie _miniGolemCookiePref = default;

    /// <summary>
    /// Called by walk animation.
    /// </summary>
    public void JumpMove(float duration)
    {
        Vector2 moveOffset = MoveDirection * (moveSpeed / 10f);
        Vector2 targetPos = (Vector2)transform.position + moveOffset;
        LeanTween.Move(gameObject, targetPos, duration);
    }

    protected override void DestroyItSelf()
    {
        SpawnMiniGolems();
        base.DestroyItSelf();
    }

    public override void Attack()
    {
        base.Attack();
        AudioManager.Instance.PlaySound("GolemCookieAttack");
    }

    private void SpawnMiniGolems()
    {
        AudioManager.Instance.PlaySound("GolemCookieExplode");

        int rankNumber = GetRankNumber();

        MiniGolemCookie miniGolemA = Instantiate(_miniGolemCookiePref, transform.position, Quaternion.identity);
        MiniGolemCookie miniGolemB = Instantiate(_miniGolemCookiePref, transform.position, Quaternion.identity);

        if (rankNumber < TileSlotManager.Instance.HigherRank)
        {
            miniGolemA.SetSplitingRank(true);
            miniGolemA.GetComponent<BoxCollider2D>().enabled = false;
            Vector2 destinationA = (Vector2)transform.position + new Vector2(0, TileSlotManager.Instance.TileSize);
            LeanTween.Move(miniGolemA.gameObject, destinationA, _splitMiniGolemsDur).SetEase(_splitGolemsEase).SetOnComplete(() => 
            {
                miniGolemA.SetSplitingRank(false);
                miniGolemA.GetComponent<BoxCollider2D>().enabled = true;
            });
        }
        if (rankNumber > TileSlotManager.Instance.LowerRank)
        {
            miniGolemB.SetSplitingRank(true);
            miniGolemB.GetComponent<BoxCollider2D>().enabled = false;
            Vector2 destinationB = (Vector2)transform.position + new Vector2(0, -TileSlotManager.Instance.TileSize);
            LeanTween.Move(miniGolemB.gameObject, destinationB, _splitMiniGolemsDur).SetEase(_splitGolemsEase).SetOnComplete(() =>
            { 
                miniGolemB.SetSplitingRank(false);
                miniGolemB.GetComponent<BoxCollider2D>().enabled = true;
            });
        }
    }

    private int GetRankNumber()
    {
        return TileSlotManager.Instance.GetRank(transform.position).RankNumber;
    }
}
