using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour, Health
{
    [SerializeField] private float maxHealth = 100;
    private EnemyHealthBar _healthBar;

    private float _currentHealth;

    private void Awake()
    {
        _currentHealth = maxHealth;
        _healthBar = GetComponent<EnemyHealthBar>();
        _healthBar.MaxHealth = maxHealth;
        _healthBar.CurrentHealth = _currentHealth;
    }


    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        _healthBar.CurrentHealth = _currentHealth;
        if (_currentHealth <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}