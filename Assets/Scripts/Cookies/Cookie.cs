using My_Utils;
using UnityEngine;

public abstract class Cookie : StateMachine, IDamageable
{
    protected Vector2 MoveDirection { get; } = Vector2.left;

    [SerializeField] private float _maxLife = default;
    [SerializeField] protected float moveSpeed = default;
    [SerializeField] protected int damage;
    [SerializeField] private LayerMask _doughnutLayer = default;

    [Space]
    [Tooltip("Mark to edit attack range.")]
    [SerializeField] private bool _editAttackRange = default;
    [ConditionalShow("_editAttackRange", true)]
    [SerializeField] private Vector2 _attackRange = default;
    [SerializeField] private Transform _attackPos = default;

    public Animator Animator { get; private set; }

    public float Life { get; set; }

    public void TakeDamage(float damageAmount)
    {
        Life -= damageAmount;

        if (Life <= 0) DestroyItSelf();
        else if (Life <= _maxLife / 2) Animator.SetTrigger("Crack");
    }

    private void DestroyItSelf()
    {
        ObjectPooler.Instance.PutToPool(GetComponent<CookiePooledObject>());
    }

    protected virtual void Awake()
    {
        Animator = GetComponent<Animator>();
        Life = _maxLife;
    }

    public override void InitializeMachine()
    {
        SetState(new CookieStateIdle(this));
    }

    public bool ShouldAttack()
    {
        return Physics2D.OverlapBox(_attackPos.position, _attackRange, 0, _doughnutLayer);
    }

    /// <summary>
    /// Called by attack animation.
    /// </summary>
    public void Attack()
    {
        Collider2D[] doughnuts = Physics2D.OverlapBoxAll(_attackPos.position, _attackRange, 0, _doughnutLayer);
        for (int i = 0; i < doughnuts.Length; i++)
        {
            if (doughnuts[i].TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(damage);
            }
        }

        if (!ShouldAttack()) FinishAttack();
    }

    private void FinishAttack()
    {
        atualState.Exit();  // Exit attack state
    }

    private void OnDrawGizmos()
    {
        DrawAttackRange();
    }

    private void DrawAttackRange()
    {
        if (!_editAttackRange || _attackPos == null) return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(_attackPos.position, _attackRange);
    }
}
