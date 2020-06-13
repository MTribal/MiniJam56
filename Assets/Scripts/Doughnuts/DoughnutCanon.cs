using UnityEngine;

public class DoughnutCanon : Doughnut
{
    [SerializeField] private float _firstSquareX = default;
    private Vector2 FirstSquarePos { get => new Vector2(_firstSquareX, transform.position.y); }


    public override bool ShouldWork()
    {
        return Physics2D.Linecast(transform.position, FirstSquarePos, enemyLayer).collider != null;
    }

    public override void Work()
    {

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
