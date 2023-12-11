using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DiscoBallManager : MonoBehaviour
{
    public static event Action OnDiscoBallHit;

    [Header("Party Config")]
    [SerializeField] private float _discoBallPartyTime = 2f;

    [Header("Light Config")]
    [Space(1)]
    [SerializeField] private Light2D _globalLight;
    [SerializeField] private float _discoLightIntensity;

    [Header("Disco Ball Config")]
    [Space(1)]
    [Tooltip("This is multiplied by DiscoBall's rotation speed")]
    [SerializeField] private float _rotationSpeedMultiplier = 1f;

    private bool _hasStartedParty = false;
    private float _defaultLightIntensity;
    private ColorSpotlight[] _colorSpotlights;

    public float DiscoBallPartyTime => _discoBallPartyTime;

    public void StartDiscoParty()
    {
        if (!_hasStartedParty)
        {
            StartCoroutine(StartDiscoPartyCoroutine());
            OnDiscoBallHit?.Invoke();
        }
    }

    private void Awake()
    {
        _defaultLightIntensity = _globalLight.intensity;
        _colorSpotlights = FindObjectsByType<ColorSpotlight>(FindObjectsSortMode.None);
    }

    private IEnumerator StartDiscoPartyCoroutine()
    {
        _hasStartedParty = true;

        SpeedUpDiscoBalls();
        DimLight();

        yield return new WaitForSeconds(_discoBallPartyTime);

        SpeedDownDiscoBalls();
        BrightLight();

        _hasStartedParty = false;
    }

    private void SpeedUpDiscoBalls()
    {
        foreach (var colorSpotlight in _colorSpotlights)
        {
            colorSpotlight.RotationSpeed *= _rotationSpeedMultiplier;
        }
    }

    private void SpeedDownDiscoBalls()
    {
        foreach (var colorSpotlight in _colorSpotlights)
        {
            colorSpotlight.RotationSpeed /= _rotationSpeedMultiplier;
        }
    }

    private void DimLight()
    {
        _globalLight.intensity = _discoLightIntensity;
    }

    private void BrightLight()
    {
        _globalLight.intensity = _defaultLightIntensity;
    }
}
