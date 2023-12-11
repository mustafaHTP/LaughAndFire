using UnityEngine;

public interface IDamagable : IHitable
{
    void TakeDamage(Vector2 damageSourceDirection, int damageAmount, float knockbackThrust);
}
