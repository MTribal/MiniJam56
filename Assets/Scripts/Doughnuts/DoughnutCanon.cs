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
        _canonGun.SetNextTarget(GetTarget());
        _canonGun.Shoot(0);
    }

    private Vector2 GetTarget()
    {
        return Physics2D.Linecast(transform.position, FirstSquarePos, enemyLayer).collider.transform.position;
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
