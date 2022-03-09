using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.Utilities;
using UnityEngine;

public class FlashingEffect : MonoBehaviour
{

    private SkinnedMeshRenderer _skinnedMeshRenderer;
    private Material[] _materials;
    private List<Color> _originalColours;

    void Awake()
    {
        _skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        _materials = _skinnedMeshRenderer.materials;
        _originalColours = _materials.Select(material => material.color).ToList();
    }

    public void StartFlashing(float flashFrequency, Color flashColor, float lifeTime)
    {
        StartCoroutine(StartFlashingCoroutine(flashFrequency, flashColor, lifeTime));
        StartCoroutine(StopCoroutineAfterTime(lifeTime));
    }


    private IEnumerator StartFlashingCoroutine(float flashFrequency, Color flashColor, float lifeTime)
    {
        while (true)
        {
            // _skinnedMeshRenderer.materials.ForEach(material => material.SetColor("_Color", flashColor));
            _skinnedMeshRenderer.materials[0].SetColor("_Color", flashColor);
            yield return new WaitForSeconds(flashFrequency);
            // _originalColours.ForEach(
            // (color, indx) =>
            // {
            // _skinnedMeshRenderer.materials[indx].SetColor("_Color", color);
            // });
            _skinnedMeshRenderer.materials[0].SetColor("_Color", _originalColours[0]);
            yield return new WaitForSeconds(flashFrequency);
        }
    }
    
    private IEnumerator StopCoroutineAfterTime(float lifeTime)
    {
        yield return new WaitForSeconds(lifeTime);
        StopAllCoroutines();
    }
}
