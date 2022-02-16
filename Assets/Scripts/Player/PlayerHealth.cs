using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private SkinnedMeshRenderer _skinnedMeshRenderer;
    public bool _isInvincible;

    private void Start()
    {
        _skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
    }

    public void TakeDamage()
    {
        if (!_isInvincible)
        {
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
