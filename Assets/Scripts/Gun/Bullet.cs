using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("VFX")]
    [SerializeField] private GameObject _bulletHitVFX;

    [Header("Bullet Config")]
    [SerializeField] private float _moveSpeed = 10f;
    [SerializeField] private int _damageAmount = 1;
    [SerializeField] private float _knockbackPower = 100f;

    private Vector2 _fireDirection;

    private Rigidbody2D _rigidBody;
    private Gun _gun;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        _rigidBody.velocity = _fireDirection * _moveSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Hit VFX
        Instantiate(_bulletHitVFX, transform.position, transform.rotation);

        //Make hit
        IHitable hitable = other.gameObject.GetComponent<IHitable>();
        hitable?.TakeHit();

        //Inflict damage
        IDamagable damagable = other.gameObject.GetComponent<IDamagable>();
        damagable?.TakeDamage(_fireDirection,_damageAmount, _knockbackPower);

        //Return to pool
        _gun.ReleaseBulletToPool(this);
    }

    public void Init(Gun gun, Vector2 bulletSpawnPos, Vector2 mousePos)
    {
        _gun = gun;
        transform.position = bulletSpawnPos;
        _fireDirection = (mousePos - bulletSpawnPos).normalized;
    }
}