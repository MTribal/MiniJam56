using My_Utils;
using My_Utils.Shooting;
using System.Collections;
using UnityEngine;

public class CanonProjectile : BaseProjectile
{
    [SerializeField] private float _upForce = 1f;
    
    private AnimationCurve _yPosCurve;

    private Vector2 _startPos;
    private Vector2 _targetPos;
    private bool _reachedTarget;

    public void StartMove(Vector2 targetpos, AnimationCurve yPosCurve)
    {
        _startPos = transform.position;
        _targetPos = targetpos;
        _yPosCurve = yPosCurve;
        StartCoroutine(Moving());
    }

    protected override void FixedUpdate()
    {
        // Not call base.FixedUpdate()
    }

    private IEnumerator Moving()
    {
        float duration = 10f / speed;
        float jumpIntensity =  Mathf.Abs((_startPos - _targetPos).x) * duration * _upForce;

        float percent = 0;
        while (percent <= 1f)
        {
            percent += Time.deltaTime / duration;
            Vector2 nextPos = MyLerp.Lerp(_startPos, _targetPos, percent, EaseType.Linear);
            Vector2 yOffset = new Vector2(0, _yPosCurve.Evaluate(percent) * jumpIntensity);
            transform.position = nextPos + yOffset;
            yield return null;
        }

        _reachedTarget = true;
        GetComponent<Animator>().SetTrigger("Destroy");
    }

    protected override void OnTriggerWithTarget(Collider2D collision)
    {
        if (!_reachedTarget) return;

        if (collision.TryGetComponent(out IDamageable damageable)) damageable.TakeDamage(damage);
    }
}
