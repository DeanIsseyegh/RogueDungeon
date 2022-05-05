using UnityEngine;

public class EnemyHealth : MonoBehaviour, Health
{
    [SerializeField] private float maxHealth = 100;
    private HealthBar _healthBar;

    private float _currentHealth;
    private Collider _collider;

    private void Awake()
    {
        _currentHealth = maxHealth;
        _healthBar = GetComponent<HealthBar>();
        _healthBar.MaxHealth = maxHealth;
        _healthBar.CurrentHealth = _currentHealth;
        _collider = GetComponent<Collider>();
    }


    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        _healthBar.CurrentHealth = _currentHealth;
        if (IsDepleted())
        {
            RemoveStatuses();
            RemoveHitBox();
        }
    }

    public bool IsDepleted()
    {
        return _currentHealth <= 0;
    }
    
    private void RemoveHitBox()
    {
        _collider.enabled = false;
    }
    
    private void RemoveStatuses()
    {
        Status[] statusArray = GetComponents<Status>();
        foreach (Status status in statusArray)
        {
            status.Remove();
        }
    }
}