using My_Utils;
using My_Utils.Audio;
using My_Utils.Shooting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CanonProjectile : BaseProjectile
{
    [SerializeField] private float _paraboleStartAngle = default;
    [SerializeField] private float _distToTargetToExplode = 0.05f;

    private Rigidbody2D _rb;
    private Vector2 _targetPos;
    private bool _causedDamage;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public override void OnSpawnFromPool()
    {
        base.OnSpawnFromPool();
        _rb.isKinematic = false;
        _causedDamage = false;
    }

    public void StartMove(Vector2 targetpos)
    {
        _targetPos = targetpos;

        Vector2 force = MyUtils.CalculateParaboleForce(transform.position, _targetPos, _paraboleStartAngle, _rb.gravityScale);
        _rb.AddForce(force, ForceMode2D.Impulse);
    }

    protected override void FixedUpdate()
    {
        if (!_causedDamage && transform.position.IsCloseTo(_targetPos, _distToTargetToExplode))
        {
            ReachedTarget();
        }
    }

    private void ReachedTarget()
    {
        AudioManager.Instance.PlaySound("CanonBallExplode");
        _rb.isKinematic = true;
        GetComponent<Animator>().SetTrigger("Destroy");

        CircleCollider2D circleCollider2D = GetComponent<CircleCollider2D>();
        Vector2 point = (Vector2)transform.position + circleCollider2D.offset;
        float radius = circleCollider2D.radius;
        Collider2D[] cookies = Physics2D.OverlapCircleAll(point, radius, targetLayers);

        foreach (Collider2D cookie in cookies)
        {
            if (cookie.TryGetComponent(out IDamageable damageable)) damageable.TakeDamage(damage);
        }

        _causedDamage = true;
    }
}
