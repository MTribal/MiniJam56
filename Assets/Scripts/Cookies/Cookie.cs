using My_Utils;
using My_Utils.Audio;
using UnityEngine;

public abstract class Cookie : StateMachine, IDamageable
{
    public Vector2 MoveDirection { get; } = Vector2.left;

    [SerializeField] private int _wheight = default;
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

    public int Wheight { get => _wheight; }
    public float MoveSpeed { get => moveSpeed; }
    public Animator Animator { get; private set; }
    public float Life { get; set; }

    private bool _isCracked;

    public void TakeDamage(float damageAmount)
    {
        Life -= damageAmount;

        if (Life <= 0) DestroyItSelf();
        else if (Life <= _maxLife / 2)
        {
            if (!_isCracked)
            {
                _isCracked = true;
                AudioManager.Instance.PlaySound("CookieCrack");
            }

            Animator.SetTrigger("Crack");
        }
    }

    protected virtual void DestroyItSelf()
    {
        GameManager.Instance.AddScore(_wheight);
        Destroy(gameObject);
    }

    protected virtual void Awake()
    {
        Animator = GetComponent<Animator>();
        Life = _maxLife;
    }

    public override void InitializeMachine()
    {
        SetState(new CookieStateWalk(this));
    }

    public bool ShouldAttack()
    {
        return Physics2D.OverlapBox(_attackPos.position, _attackRange, 0, _doughnutLayer);
    }

    /// <summary>
    /// Called by attack animation.
    /// </summary>
    public virtual void Attack()
    {
        if (!GameManager.Instance.IsPlaying) return;

        Collider2D[] doughnuts = Physics2D.OverlapBoxAll(_attackPos.position, _attackRange, 0, _doughnutLayer);
        for (int i = 0; i < doughnuts.Length; i++)
        {
            if (doughnuts[i].TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(damage);
                break;
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
