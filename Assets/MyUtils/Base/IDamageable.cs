namespace My_Utils
{
    public interface IDamageable
    {
        int Life { get; }

        void TakeDamage(int damageAmount);

        void DestroyItSelf();
    }
}