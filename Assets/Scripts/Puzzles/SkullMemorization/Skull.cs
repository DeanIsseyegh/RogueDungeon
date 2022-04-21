using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class Skull : MonoBehaviour
{
    [SerializeField] private GameObject lightUpFx;
    [SerializeField] private float lightUpDuration;
    [SerializeField] private GameObject candle;
    [SerializeField] private GameObject notReadyLight;
    [SerializeField] private GameObject readyLight;

    private MemorizationPuzzleManager _puzzleManager;
    private bool _isSkullHittable;

    public bool HasLitUp { get; set; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Spell"))
        {
            if (_isSkullHittable)
            {
                _puzzleManager.NotifySkullHit(this);
                _isSkullHittable = false;
            }
        }
    }
    
    public void LightUp()
    {
        lightUpFx.SetActive(true);
        StartCoroutine(LightOff(lightUpDuration));
    }
    
    public void MarkSuccessfulHit()
    {
        candle.SetActive(true);
    }

    private IEnumerator LightOff(float delay)
    {
        yield return new WaitForSeconds(delay);
        lightUpFx.SetActive(false);
    }

    public void MakeHittable()
    {
        _isSkullHittable = true;
    }
    
    public void MakeUnHittable()
    {
        _isSkullHittable = false;
    }

    public void Register(MemorizationPuzzleManager puzzleManager)
    {
        _puzzleManager = puzzleManager;
    }

    public void Unmark()
    {
        _isSkullHittable = false;
        candle.SetActive(false);
    }

    public void MarkUnready()
    {
        notReadyLight.SetActive(true);
        readyLight.SetActive(false);
    }
    
    public void MarkReady()
    {
        notReadyLight.SetActive(false);
        readyLight.SetActive(true);
    }

    public void MarkFinished()
    {
        notReadyLight.SetActive(false);
        readyLight.SetActive(false);
        _isSkullHittable = false;
        candle.SetActive(true);
    }

}