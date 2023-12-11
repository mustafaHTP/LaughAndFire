using Cinemachine;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class Gun : MonoBehaviour
{
    public static event Action OnShoot;

    [SerializeField] private Transform _bulletSpawnPoint;
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private float _gunFireCooldown;
    [SerializeField] private float _muzzleFlashTime;
    [SerializeField] private GameObject _muzzleFlash;

    //Animation
    private static readonly int FireHash = Animator.StringToHash("Fire");
    private Animator _animator;

    //Screen shake
    private CinemachineImpulseSource _impulseSource;

    //Muzzle light
    private Coroutine _muzzleFlashCoroutine;

    private float _lastFireTime = 0f;
    private Vector2 _mousePosition;
    private ObjectPool<Bullet> _bulletPool;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    private void OnEnable()
    {
        OnShoot += ShootProjectile;
        OnShoot += ResetLastFireTime;
        OnShoot += FireAnimation;
        OnShoot += ShakeScreen;
        OnShoot += EnableMuzzleFlash;
    }

    private void OnDisable()
    {
        OnShoot -= ShootProjectile;
        OnShoot -= ResetLastFireTime;
        OnShoot -= FireAnimation;
        OnShoot -= ShakeScreen;
        OnShoot -= EnableMuzzleFlash;
    }

    private void Start()
    {
        CreateBulletPool();
    }

    private void Update()
    {
        RotateGun();
        Shoot();
    }

    private void RotateGun()
    {
        _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //Vector2 direction = mouseWorldPos - transform.position;
        Vector2 direction = PlayerController.Instance.transform.InverseTransformPoint(_mousePosition);
        float angleBetweenDirectionAndThis = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.localRotation = Quaternion.Euler(0f, 0f, angleBetweenDirectionAndThis);
    }

    #region Bullet Pool Methods

    private void CreateBulletPool()
    {
        _bulletPool = new(CreateBullet, OnGetBullet, OnReleaseBullet, OnDestroyBullet, false, 20, 40);
    }

    private Bullet CreateBullet()
    {
        return Instantiate(_bulletPrefab);
    }

    private void OnGetBullet(Bullet bullet)
    {
        bullet.gameObject.SetActive(true);
    }

    private void OnReleaseBullet(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
    }

    private void OnDestroyBullet(Bullet bullet)
    {
        Destroy(bullet.gameObject);
    }

    public void ReleaseBulletToPool(Bullet bullet)
    {
        _bulletPool.Release(bullet);
    }

    #endregion

    private void ShakeScreen()
    {
        _impulseSource.GenerateImpulse();
    }

    private void Shoot()
    {
        if (Input.GetMouseButton(0) && Time.time >= _lastFireTime + _gunFireCooldown)
        {
            OnShoot?.Invoke();
        }
    }

    private void ResetLastFireTime() => _lastFireTime = Time.time;

    private void ShootProjectile()
    {
        Bullet newBullet = _bulletPool.Get();
        newBullet.Init(this, _bulletSpawnPoint.position, _mousePosition);
    }

    private void FireAnimation()
    {
        _animator.Play(FireHash, 0, 0f);
    }

    private void EnableMuzzleFlash()
    {
        if (_muzzleFlashCoroutine != null)
        {
            StopCoroutine(_muzzleFlashCoroutine);
        }

        _muzzleFlashCoroutine = StartCoroutine(EnableMuzzleFlashCoroutine());
    }

    private IEnumerator EnableMuzzleFlashCoroutine()
    {
        _muzzleFlash.SetActive(true);
        yield return new WaitForSeconds(_gunFireCooldown);
        _muzzleFlash.SetActive(false);
    }
}
