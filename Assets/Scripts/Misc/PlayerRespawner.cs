using Cinemachine;
using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class PlayerRespawner : MonoBehaviour
{
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    [SerializeField] private Image _fadeImage;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private float _respawnDuration = 5f;

    private event Action _onRespawnEnd;
    private event Action _onRespawnStart;
    private GameObject _playerInstance;

    private void OnEnable()
    {
        Health.OnPlayerDeath += RespawnPlayer;
        _onRespawnStart += FadeOut;
        _onRespawnEnd += ReassignPlayerToCamera;
        _onRespawnEnd += FadeIn;
    }

    private void OnDisable()
    {
        Health.OnPlayerDeath -= RespawnPlayer;
        _onRespawnStart -= FadeOut;
        _onRespawnEnd -= ReassignPlayerToCamera;
        _onRespawnEnd -= FadeIn;
    }

    private void RespawnPlayer()
    {
        _onRespawnStart?.Invoke();
        StartCoroutine(RespawnPlayerCoroutine());
    }

    private IEnumerator RespawnPlayerCoroutine()
    {
        yield return new WaitForSeconds(_respawnDuration);

        _playerInstance = Instantiate(
            _playerPrefab,
            _spawnPoint.position,
            Quaternion.identity);

        _onRespawnEnd?.Invoke();
    }

    private void ReassignPlayerToCamera()
    {
        _virtualCamera.Follow = _playerInstance.transform;
    }

    private void FadeOut()
    {
        StartCoroutine(FadeOutCoroutine());
    }

    private IEnumerator FadeOutCoroutine()
    {
        float elapsedTime = 0f;
        while (elapsedTime < _respawnDuration)
        {
            elapsedTime += Time.deltaTime;

            Color fadeColor = _fadeImage.color;
            fadeColor.a = Mathf.Lerp(0f, 1f, elapsedTime / (_respawnDuration / 2f));
            _fadeImage.color = fadeColor;

            yield return null;
        }
    }

    private void FadeIn()
    {
        StartCoroutine(FadeInCoroutine());
    }

    private IEnumerator FadeInCoroutine()
    {
        float elapsedTime = 0f;
        while (elapsedTime < _respawnDuration)
        {
            elapsedTime += Time.deltaTime;

            Color fadeColor = _fadeImage.color;
            fadeColor.a = Mathf.Lerp(1f, 0, elapsedTime / (_respawnDuration / 2f));
            _fadeImage.color = fadeColor;

            yield return null;
        }
    }
}
