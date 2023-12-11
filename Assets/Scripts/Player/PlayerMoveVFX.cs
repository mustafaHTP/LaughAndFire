using UnityEngine;

public class PlayerMoveVFX : MonoBehaviour
{
    [SerializeField] private ParticleSystem _playerMoveVFX;

    private void Update()
    {
        if (PlayerController.Instance.IsGrounded())
        {
            PlayVFX();
        }
        else
        {
            StopVFX();
        }
    }

    private void PlayVFX()
    {
        if (!_playerMoveVFX.isPlaying)
        {
            _playerMoveVFX.Play();
        }
    }

    private void StopVFX()
    {
        if (_playerMoveVFX.isPlaying)
        {
            _playerMoveVFX.Stop();
        }
    }

}
