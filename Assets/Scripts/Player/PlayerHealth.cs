using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, Health
{
    [SerializeField] private float maxHealth = 100;
    [ShowInInspector] private float _currentHealth;
    [SerializeField] private float invincibilityAfterHitTimer = 0.25f;
    private SkinnedMeshRenderer _skinnedMeshRenderer;
    private bool _isInvincible;
    private UIManager _uiManager;

    private void Start()
    {
        _skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        _currentHealth = maxHealth;
        _uiManager = GameObject.FindWithTag("UIManager").GetComponent<UIManager>();
        _uiManager.SetMaxHealth(maxHealth);
        _uiManager.SetCurrentHealth(_currentHealth);
    }

    public void TakeDamage(float damage)
    {
        if (!_isInvincible)
        {
            _currentHealth = Mathf.Clamp(_currentHealth - damage, 0, maxHealth);
            _uiManager.SetCurrentHealth(_currentHealth);
            _isInvincible = true;
            StartCoroutine(HitFlashEffect(5));
        }
    }

    public bool IsDepleted()
    {
        return _currentHealth <= 0;
    }

    public void AddMaxHp(float hpToAdd)
    {
        maxHealth += hpToAdd;
        _currentHealth += hpToAdd;
        if (_currentHealth > maxHealth)
        {
            _currentHealth = maxHealth;
        }
        _uiManager.SetMaxHealth(maxHealth);
        _uiManager.SetCurrentHealth(_currentHealth);
    }

    private IEnumerator HitFlashEffect(int numOfTimes)
    {
        for (int i = 0; i < numOfTimes; i++)
        {
            _skinnedMeshRenderer.enabled = !_skinnedMeshRenderer.enabled;
            yield return new WaitForSeconds(invincibilityAfterHitTimer / numOfTimes);
        }

        _skinnedMeshRenderer.enabled = true;
        _isInvincible = false;
    }
}