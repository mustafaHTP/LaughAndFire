using System;
using UnityEngine;

public class Health : MonoBehaviour, IDamagable
{
    [SerializeField] private GameObject _splatterPrefab;
    [SerializeField] private GameObject _deathVFX;
    [SerializeField] private int _startingHealth = 3;
    [SerializeField] private bool _isPlayer;

    public static event Action<Health> OnDeath;
    public static event Action OnPlayerDeath;
    public static event Action OnDeathSFX;

    public GameObject SplatterPrefab => _splatterPrefab;
    public GameObject DeathVFX => _deathVFX;

    private int _currentHealth;
    private Knockback _knockback;
    private Flash _flash;

    private void Awake()
    {
        _knockback = GetComponent<Knockback>();
        _flash = GetComponent<Flash>();
    }

    private void Start()
    {
        ResetHealth();
    }

    private void ResetHealth()
    {
        _currentHealth = _startingHealth;
    }

    private void DecreaseHealth(int amount)
    {
        _currentHealth -= amount;

        if (_currentHealth <= 0)
        {
            OnDeath?.Invoke(this);
            OnDeathSFX?.Invoke();

            if (_isPlayer)
            {
                OnPlayerDeath?.Invoke();
            }

            Destroy(gameObject);
        }
    }

    public void TakeDamage(Vector2 damageSourceDirection, int damageAmount, float knockbackThrust)
    {
        DecreaseHealth(damageAmount);
        _knockback.ApplyKnockback(
            damageSourceDirection,
            knockbackThrust);
    }

    public void TakeHit()
    {
        _flash.OnFlashStart?.Invoke();
    }
}
