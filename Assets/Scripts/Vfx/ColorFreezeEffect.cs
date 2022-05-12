using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ColorFreezeEffect : MonoBehaviour
{
    private Renderer _renderer;
    private Material[] _materials;
    private List<Color> _originalColours;
    private bool _shouldStartColorFreeze;
    private float _freezeDuration;
    private float _freezeTimer;
    private Color _freezeColor;

    void Awake()
    {
        _renderer = GetComponentInChildren<Renderer>();
        _materials = _renderer.materials;
        _originalColours = _materials.Select(material => material.color).ToList();
    }

    private void Update()
    {
        if (_shouldStartColorFreeze)
        {
            _freezeTimer -= Time.deltaTime;
            float lerp = Mathf.Clamp(_freezeTimer / _freezeDuration, 0, 2.5f);
            float colorIntensity = (lerp * 4) + 1.0f;
            _renderer.material.color = (_freezeColor * colorIntensity);
            if (_freezeTimer < 0)
            {
                _renderer.material.color = _originalColours[0];
                _shouldStartColorFreeze = false;
                Destroy(this);
            }
        }
    }

    public void StartColorFreeze(Color flashColor, float lifeTime)
    {
        _shouldStartColorFreeze = true;
        _freezeDuration = lifeTime;
        _freezeTimer = lifeTime;
        _freezeColor = flashColor;
        StartCoroutine(StopCoroutineAfterTime(lifeTime));
    }
    
    private IEnumerator StopCoroutineAfterTime(float lifeTime)
    {
        yield return new WaitForSeconds(lifeTime);
        StopAllCoroutines();
    }
}
