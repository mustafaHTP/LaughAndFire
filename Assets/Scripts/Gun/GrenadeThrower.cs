using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeThrower : MonoBehaviour
{
    public static event Action OnThrow;

    [SerializeField] private GameObject _grenadePrefab;
    [SerializeField] private Transform _grenadeSpawnPoint;
    [SerializeField] private float _throwCooldown = 2f;
    [SerializeField] private float _throwForce = 1000f;
    [SerializeField] private float _torqueForce = 100f;

    private Coroutine _throwGrenadeCR;

    private void Update()
    {
        if (PlayerController.Instance.ThrowGrenadeInput && _throwGrenadeCR == null)
        {
            _throwGrenadeCR = StartCoroutine(ThrowGrenade());
        }
    }

    private IEnumerator ThrowGrenade()
    {
        OnThrow?.Invoke();

        Vector2 _throwDirection = 
            (Utils.GetMousePositionInWorld() - (Vector2)transform.position).normalized;

        GameObject grenadeInstance = Instantiate(
            _grenadePrefab,
            _grenadeSpawnPoint.position,
            Quaternion.identity);

        if (grenadeInstance.TryGetComponent(out Rigidbody2D rigidbody))
        {
            rigidbody.AddForce(_throwForce * _throwDirection, ForceMode2D.Force);
            rigidbody.AddTorque(_torqueForce);
        }

        yield return new WaitForSeconds(_throwCooldown);

        _throwGrenadeCR = null;
    }
}
