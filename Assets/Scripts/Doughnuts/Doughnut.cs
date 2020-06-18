using My_Utils;
using System.Collections;
using UnityEngine;

public abstract class Doughnut : StateMachine, IDamageable
{
    [Tooltip("The max life. Also start life.")]
    [SerializeField] private float _maxLife = default;

    [Tooltip("Should be longer than the work animation.")]
    [SerializeField] private float _workCooldown = default;
    public float WorkCooldown { get => _workCooldown; }

    [SerializeField] protected LayerMask enemyLayer;
    public int EnemyLayer { get => enemyLayer; }

    public bool CanWork { get; private set; }

    public Animator Animator { get; private set; }

    public float Life { get; private set; }

    public void TakeDamage(float damageAmount)
    {
        Life -= damageAmount;
        if (Life <= 0) DestroyItSelf();
    }

    public void DestroyItSelf()
    {
        TileSlotManager.Instance.GetTileSlot(transform.position).Disoccupe();
        Destroy(gameObject);
    }

    protected virtual void Awake()
    {
        Animator = GetComponent<Animator>();
        Life = _maxLife;
    }

    protected override void Start()
    {
        ResetCanWork();
        base.Start();
    }

    public override void InitializeMachine()
    {
        SetState(new DoughnutIdleState(this));
    }

    public abstract bool ShouldWork();

    public void ResetCanWork()
    {
        StopCoroutine("ResetinCanAttack");
        StartCoroutine(ResetinCanAttack());
    }

    private IEnumerator ResetinCanAttack()
    {
        CanWork = false;
        yield return new WaitForSeconds(_workCooldown);
        CanWork = true;
    }


    /// <summary>
    /// Called by work animation.
    /// </summary>
    public abstract void Work();


    /// <summary>
    /// Called by work animation when it finishes.
    /// </summary>
    public void OnWorkFinished()
    {
        if (_workCooldown != 0 || !ShouldWork())
            atualState.Exit();  // Exit work state
    }
}
