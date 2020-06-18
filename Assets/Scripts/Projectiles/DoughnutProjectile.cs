using My_Utils;
using My_Utils.Audio;
using My_Utils.Shooting;
using UnityEngine;

public class DoughnutProjectile : BaseProjectile
{
    protected override void OnTriggerWithTarget(Collider2D collision)
    {
        AudioManager.Instance.PlaySound("DoughnutBoxFireExplode");

        if (collision.TryGetComponent(out IDamageable damageable)) damageable.TakeDamage(damage);

        GetComponent<Animator>().SetTrigger("Destroy");
        shutingDown = true;
    }
}
