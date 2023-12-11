using System;
using System.Collections;
using UnityEngine;

public class Flash : MonoBehaviour
{
    [SerializeField] private Material _defaultMaterial;
    [SerializeField] private Material _flashMaterial;
    [SerializeField] private float _flashDuration = 0.1f;

    private SpriteRenderer[] _spriteRenderers;
    private ColorChanger _colorChanger;
    public Action OnFlashStart;
    public Action OnFlashEnd;

    private void Awake()
    {
        _colorChanger = GetComponent<ColorChanger>();
        _spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
    }

    private void OnEnable()
    {
        OnFlashStart += StartFlash;
        OnFlashEnd += SetDefaultMaterial;
    }

    private void OnDisable()
    {
        OnFlashEnd -= StartFlash;
        OnFlashEnd -= SetDefaultMaterial;
    }

    private void StartFlash()
    {
        StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        foreach (var spriteRenderer in _spriteRenderers)
        {
            spriteRenderer.material = _flashMaterial;
            if (_colorChanger)
            {
                _colorChanger.SetColor(Color.white);
            }
        }

        yield return new WaitForSeconds(_flashDuration);
        OnFlashEnd?.Invoke();
    }

    private void SetDefaultMaterial()
    {
        foreach (var spriteRenderer in _spriteRenderers)
        {
            spriteRenderer.material = _defaultMaterial;
            if (_colorChanger)
            {
                _colorChanger.SetColor(_colorChanger.DefaultColor);
            }
        }
    }
}
