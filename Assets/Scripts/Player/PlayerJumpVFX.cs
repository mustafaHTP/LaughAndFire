using UnityEngine;

public class PlayerJumpVFX : MonoBehaviour
{
    [SerializeField] private ParticleSystem _playerJumpVFX;

    private void OnEnable()
    {
        PlayerController.OnJump += PlayVFX;
    }

    private void OnDisable()
    {
        PlayerController.OnJump -= PlayVFX;
    }

    private void PlayVFX()
    {
        _playerJumpVFX.Play();
    }
}
