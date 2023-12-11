using UnityEngine;

public class ColorSpotlight : MonoBehaviour
{
    [SerializeField] private GameObject _spotlight;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _maxRotation;

    private float _currentZAngle;

    public float RotationSpeed
    {
        get => _rotationSpeed;
        set => _rotationSpeed = value;
    }

    private void Start()
    {
        RandomStartingRotation();
    }

    private void Update()
    {
        RotateSpotlight();
    }

    private void RotateSpotlight()
    {
        _currentZAngle += Time.deltaTime * _rotationSpeed;

        _spotlight.transform.localRotation = Quaternion.Euler(
            0f,
            0f,
            Mathf.PingPong(_currentZAngle, _maxRotation));
    }

    private void RandomStartingRotation()
    {
        float startingRotation = Random.Range(-1f * _maxRotation, _maxRotation);
        _spotlight.transform.localRotation = Quaternion.Euler(
            0f,
            0f,
            startingRotation);
    }
}
