using My_Utils.Shooting;
using UnityEngine;

public class CanonGun : BaseGun
{
    public AnimationCurve _yPosCurve = default;
    private Vector2 _nextTarget;


    /// <summary>
    /// Set the next canon projectile target. This should be called before Shoot().
    /// </summary>
    public void SetNextTarget(Vector2 nextTarget)
    {
        _nextTarget = nextTarget;
    }

    public override BaseProjectile Shoot(float gunAngle)
    {
        CanonProjectile canonProjectile = (CanonProjectile) base.Shoot(gunAngle);
        canonProjectile.StartMove(_nextTarget, _yPosCurve);

        return canonProjectile;
    }
}
