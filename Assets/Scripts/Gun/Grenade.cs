using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Grenade : MonoBehaviour
{
    private event Action _onToggleGrenadeLight;
    private event Action _onExplode;

    [SerializeField] private float _lifeTime = 10f;
    [SerializeField] private float _grenadeLightInterval = 1f;
    [SerializeField] private float _explosionRadius = 3f;
    [SerializeField] private float _knockBackStrength = 100f;
    [SerializeField] private int _damageAmount = 3;
    [SerializeField] private LayerMask _enemyLayerMask;
    [SerializeField] private Light2D _grenadeLight;
    [SerializeField] private ParticleSystem _grenadeExplosionVFX;

    private float _lifeTimeCounter = 0f;
    private CinemachineImpulseSource _impulseSource;

    private void Awake()
    {
        _impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    private void OnEnable()
    {
        _onExplode += InstantitateGrenadeExplosionVFX;
        _onExplode += ShakeScreen;
        _onExplode += DamageNearby;
        //Break design pattern
        _onExplode += AudioManager.Instance.Grenade_OnExplosion;
        _onToggleGrenadeLight += AudioManager.Instance.Grenade_OnToggleGrenadeLight;
    }

    private void OnDisable()
    {
        _onExplode -= InstantitateGrenadeExplosionVFX;
        _onExplode -= ShakeScreen;
        _onExplode -= DamageNearby;
        _onExplode -= AudioManager.Instance.Grenade_OnExplosion;
        _onToggleGrenadeLight -= AudioManager.Instance.Grenade_OnToggleGrenadeLight;
    }

    private void Start()
    {
        StartCoroutine(ToggleGrenadeLight());
    }

    private void Update()
    {
        _lifeTimeCounter += Time.deltaTime;

        if(_lifeTimeCounter > _lifeTime)
        {
            Explode();
        }
    }

    private void Explode()
    {
        _onExplode?.Invoke();
        Destroy(gameObject);
    }

    private IEnumerator ToggleGrenadeLight()
    {
        while (true)
        {
            _grenadeLight.enabled = false;

            yield return new WaitForSeconds(_grenadeLightInterval);

            _grenadeLight.enabled = true;
            _onToggleGrenadeLight?.Invoke();

            yield return new WaitForSeconds(_grenadeLightInterval);
        }
    }

    private void InstantitateGrenadeExplosionVFX()
    {
        Instantiate(_grenadeExplosionVFX, transform.position, Quaternion.identity);
    }

    private void ShakeScreen()
    {
        _impulseSource.GenerateImpulse();
    }

    private void DamageNearby()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, _explosionRadius, _enemyLayerMask);

        foreach (Collider2D hit in hits)
        {
            if(hit.TryGetComponent(out IDamagable damagable))
            {
                Debug.Log("Grenade DAMAGE");
                damagable.TakeDamage(transform.position, _damageAmount, _knockBackStrength);
            }

            if (hit.TryGetComponent(out IHitable hitable))
            {
                hitable.TakeHit();
            }
        }
    }
}
