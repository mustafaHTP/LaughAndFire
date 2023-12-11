using UnityEngine;

public class PlayerLean : MonoBehaviour
{
    [SerializeField] private Transform _playerSprite;
    [SerializeField] private Transform _hatSprite;
    [SerializeField] private Transform _jetpackSprite;
    [SerializeField] private float _leanAmount;
    [SerializeField] private float _leanSpeed;

    private float _targetLean;

    private void Update()
    {
        if (PlayerController.Instance.MoveInput.x < 0f)
        {
            _targetLean = _leanAmount;
        }
        else if (PlayerController.Instance.MoveInput.x > 0f)
        {
            _targetLean = -1f * _leanAmount;
        }
        else
        {
            _targetLean = 0f;
        }

        LeanSprite(_playerSprite, _targetLean);
        LeanSprite(_jetpackSprite, _targetLean);
        LeanSprite(_hatSprite, -1f * _targetLean);
    }

    private void LeanSprite(Transform spriteTransform, float targetLean)
    {
        Quaternion currentRotation = spriteTransform.rotation;
        Quaternion targetRotation = Quaternion.Euler(
            currentRotation.eulerAngles.x,
            currentRotation.eulerAngles.y,
            targetLean);

        spriteTransform.rotation = Quaternion.Lerp(
            currentRotation,
            targetRotation,
            _leanSpeed * Time.deltaTime);
    }
}
