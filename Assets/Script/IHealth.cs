public interface IHealth
{
    public float Health { get; }
    public float MaxHealth { get; }

    public void TakeDamage(float damage);
    public void Heal(float healing);
}
