public interface IDamager
{
    void Attack();
    void DoDamage(IDamageable target, Damage damage);
}
