using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MegaKill : MonoBehaviour
{
    public static event Action OnMegaKill;

    [Tooltip("Time that to be achieved mega kill")]
    [SerializeField] private float _megaKillTime = 1f;
    [SerializeField] private int _megaKillNumber = 2;

    private float _megaKillCountdown;
    private int _killCounter = 0;
    private bool _isCounting = false;

    private void Awake()
    {
        _megaKillCountdown = _megaKillTime;
    }

    private void OnEnable()
    {
        Health.OnDeath += IncreaseKillNumber;
    }

    private void OnDisable()
    {
        Health.OnDeath -= IncreaseKillNumber;
    }


    private void Update()
    {
        if (_isCounting)
        {
            _megaKillCountdown -= Time.deltaTime;

            if(_killCounter == _megaKillNumber)
            {
                OnMegaKill?.Invoke();

                _isCounting = false;

                ResetKillCounter();
                ResetMegaKillCountdown();
            }
        }

        if(_megaKillCountdown < 0f)
        {
            _isCounting = false;

            ResetKillCounter();
            ResetMegaKillCountdown();
        }
    }

    private void IncreaseKillNumber(Health health)
    {
        if (!_isCounting)
            _isCounting = true;
        ++_killCounter;
    }

    private void ResetKillCounter() => _killCounter = 0;
    private void ResetMegaKillCountdown() => _megaKillCountdown = _megaKillTime;
}
