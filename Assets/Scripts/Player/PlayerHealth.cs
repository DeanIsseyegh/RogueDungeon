using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, Health
{
    [SerializeField] private float maxHealth = 100;
    private float _currentHealth;
    private SkinnedMeshRenderer _skinnedMeshRenderer;
    private bool _isInvincible;

    private void Start()
    {
        _skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        _currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (!_isInvincible)
        {
            _currentHealth -= damage;
            _isInvincible = true;
            StartCoroutine(HitFlashEffect(5));
        }
    }

    private IEnumerator HitFlashEffect(int numOfTimes)
    {
        for (int i = 0; i < numOfTimes; i++)
        {
            _skinnedMeshRenderer.enabled = !_skinnedMeshRenderer.enabled;
            yield return new WaitForSeconds(0.3f);
        }

        _skinnedMeshRenderer.enabled = true;
        _isInvincible = false;
    }
}
