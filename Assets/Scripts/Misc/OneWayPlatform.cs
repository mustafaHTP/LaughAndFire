using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayPlatform : MonoBehaviour
{
    [SerializeField] private float _disableColliderTime = 1f;

    private bool _playerOnPlatform;
    private Collider2D _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        DetectPlayerInput();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerController playerController))
        {
            _playerOnPlatform = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerController playerController))
        {
            _playerOnPlatform = false;
        }
    }

    private void DetectPlayerInput()
    {
        if (!_playerOnPlatform) return;

        if (PlayerController.Instance.MoveInput.y < 0f)
        {
            StartCoroutine(IgnoreCollisionToPlayer());
        }
    }

    private IEnumerator IgnoreCollisionToPlayer()
    {
        Collider2D[] playerColliders = PlayerController.Instance.GetComponents<Collider2D>();

        foreach (var playerCollider in playerColliders)
        {
            Physics2D.IgnoreCollision(playerCollider, _collider);
        }

        yield return new WaitForSeconds(_disableColliderTime);

        foreach (var playerCollider in playerColliders)
        {
            Physics2D.IgnoreCollision(playerCollider, _collider, false);
        }

    }
}
