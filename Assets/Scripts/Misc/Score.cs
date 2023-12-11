using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    [SerializeField] private int _scoreIncreaseAmount;

    private TextMeshProUGUI _scoreTMP;
    private int _score = 0;

    private void Awake()
    {
        _scoreTMP = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        Health.OnDeath += IncreaseScore; 
    }

    private void OnDisable()
    {
        Health.OnDeath -= IncreaseScore; 
    }

    private void IncreaseScore(Health health)
    {
        _score += _scoreIncreaseAmount;
        _scoreTMP.text = _score.ToString("D3");
    }
}
