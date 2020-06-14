using My_Utils;
using UnityEngine;

public class DoughnutSaw : Doughnut
{
    [Space]
    [Tooltip("Amount of damage to cause. (6x by animation)")]
    [SerializeField] private float damage = default;

    [Tooltip("Mark to edit the attack range.")]
    [SerializeField] private bool _editAttackRange = default;
    [ConditionalShow("_editAttackRange", true)]
    [SerializeField] private Vector2 _attackRange = default;
    [SerializeField] private Transform _attackPos = default;

    public override bool ShouldWork()
    {
        return Physics2D.OverlapBox(_attackPos.position, _attackRange, 0, enemyLayer);
    }

    public override void Work()
    {
        Collider2D[] enemies = Physics2D.OverlapBoxAll(_attackPos.position, _attackRange, 0, enemyLayer);
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i].TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(damage);
            }
        }
    }

    private void OnDrawGizmos()
    {
        DrawAttackRange();
    }

    private void DrawAttackRange()
    {
        if (_attackPos == null || !_editAttackRange) return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(_attackPos.position, _attackRange);
    }
}
