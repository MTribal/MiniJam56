using UnityEngine;

public class Granulator : Doughnut
{
    [Tooltip("Amount of granulate producted by each burst.")]
    [SerializeField] private int _granulateProducted = default;

    [SerializeField] private ParticleSystem _granulateBurst = default;

    public override bool ShouldWork()
    {
        return true; // Always granulate if can.
    }

    public override void Work()
    {
        _granulateBurst.Play();
        GameManager.Instance.Deposit(_granulateProducted);
    }
}
