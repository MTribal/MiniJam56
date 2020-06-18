using My_Utils.Audio;
using My_Utils.Shooting;
using UnityEngine;

public class DoughnutBox : Doughnut
{
    [SerializeField] private float _firstSquareX = default;
    private Vector2 FirstSquarePos { get => new Vector2(_firstSquareX, transform.position.y); }

    [SerializeField] private BaseGun _gun = default;

    
    public override bool ShouldWork()
    {
        return Physics2D.Linecast(transform.position, FirstSquarePos, enemyLayer).collider != null;
    }

    public override void Work()
    {
        AudioManager.Instance.PlaySound("DoughnutBoxFire");
        _gun.Shoot(0);
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
