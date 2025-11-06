using System;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour, IDamageable
{
    public float CurrentLife { get; set; }
    public float MaxLife { get; set; }

    [Header("Player Settings")]
    [SerializeField] private float health = 100f;
    [SerializeField] private string tagDamage;

    public event Action<float> OnHealthChanged; 
    public event Action OnPlayerDied;

    private bool _isDead = false; 

    private void Start()
    {
        MaxLife = health;
        CurrentLife = MaxLife;
        _isDead = false;
        
        OnHealthChanged?.Invoke(CurrentLife);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isDead) return;

        if (other.gameObject.CompareTag(tagDamage))
        {
            CurrentLife -= 1f; 

            if (CurrentLife < 0) CurrentLife = 0;

            OnHealthChanged?.Invoke(CurrentLife);
            
            if (CurrentLife <= 0)
            {
                Die();
            }
        }
    }

    private void Die()
    {
        _isDead = true;
        
        OnPlayerDied?.Invoke();
        
        Destroy(gameObject);
    }
}