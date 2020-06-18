using My_Utils.Audio;
using UnityEngine;

[RequireComponent(typeof(CanonGun))]
public class DoughnutCanon : Doughnut
{
    [SerializeField] private float _firstSquareX = default;
    private Vector2 FirstSquarePos { get => new Vector2(_firstSquareX, transform.position.y); }

    private CanonGun _canonGun;

    protected override void Awake()
    {
        base.Awake();
        _canonGun = GetComponent<CanonGun>();
    }

    public override bool ShouldWork()
    {
        return Physics2D.Linecast(transform.position, FirstSquarePos, enemyLayer).collider != null;
    }

    public override void Work()
    {
        if (GetTarget(out Vector2 target))
        {
            AudioManager.Instance.PlaySound("DoughnutCanonFire");
            _canonGun.SetNextTarget(target);
            _canonGun.Shoot(0);
        }
    }

    private bool GetTarget(out Vector2 target)
    {
        Collider2D collider2D = Physics2D.Linecast(transform.position, FirstSquarePos, enemyLayer).collider;
        if (collider2D != null)
        {
            target = collider2D.transform.position;
            return true;
        }

        target = new Vector2();
        return false;
    }

    private void OnDrawGizmos()
    {
        DrawFirstSquarePos();
    }

    private void DrawFirstSquarePos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(new Vector2(_firstSquareX, transform.position.y), 0.1f);
    }
}
