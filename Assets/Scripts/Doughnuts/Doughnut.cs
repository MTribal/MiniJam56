using My_Utils;
using System.Collections;
using UnityEngine;

public abstract class Doughnut : StateMachine
{
    [SerializeField] private float _workCooldown = default;
    public float WorkCooldown { get => _workCooldown; }

    [SerializeField] protected LayerMask enemyLayer;
    public int EnemyLayer { get => enemyLayer; }

    public bool CanWork { get; private set; }

    public Animator Animator { get; private set; }

    private void Awake()
    {
        Animator = GetComponent<Animator>();
    }

    private void Start()
    {
        ResetCanWork();
        InitializeMachine();
    }

    public override void InitializeMachine()
    {
        SetState(new DoughnutIdleState(this));
    }

    public abstract bool ShouldWork();

    public void ResetCanWork()
    {
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
        if (!CanWork)
            atualState.Exit(); // AtualState == work state
    }
}
