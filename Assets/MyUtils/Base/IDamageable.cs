namespace My_Utils
{
    public interface IDamageable
    {
        float Life { get; }

        void TakeDamage(float damageAmount);
    }
}