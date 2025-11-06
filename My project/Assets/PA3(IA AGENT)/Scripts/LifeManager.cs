using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LifeManager : MonoBehaviour
{
    [Header("Life Player UI")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject LosePanel;
    
    private PlayerBehaviour _player;

    private void Awake()
    {
        _player = FindFirstObjectByType<PlayerBehaviour>();

        if (_player == null)
        {
            Debug.LogError("¡LifeManager no pudo encontrar un PlayerBehaviour en la escena!");
            return; 
        }

        if (LosePanel) LosePanel.SetActive(false);
    }

    private void OnEnable()
    {
        if (_player != null)
        {
            _player.OnHealthChanged += HandleHealthChanged;
            _player.OnPlayerDied += HandlePlayerDied;
        }
    }

    private void OnDisable()
    {
        if (_player != null)
        {
            _player.OnHealthChanged -= HandleHealthChanged;
            _player.OnPlayerDied -= HandlePlayerDied;
        }
    }
    
    private void Start()
    {
        if (_player != null)
        {
            UpdateTxt(_player.CurrentLife);
        }
    }

    private void HandleHealthChanged(float newHealth)
    {
        UpdateTxt(newHealth);
    }

    private void HandlePlayerDied()
    {
        if (LosePanel != null)
        {
            LosePanel.SetActive(true);
        }
        
        Debug.Log("You Lose");
    }

    private void UpdateTxt(float health)
    {
        if (scoreText != null)
        {
            scoreText.text = Mathf.CeilToInt(health).ToString();
        }
    }
}