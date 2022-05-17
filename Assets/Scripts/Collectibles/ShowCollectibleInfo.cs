using System;
using UnityEngine;

public class ShowCollectibleInfo : MonoBehaviour
{
    [SerializeField] private Collectible collectible;
    private UIManager _uiManager;

    private void Awake()
    {
        _uiManager = GameObject.FindWithTag("UIManager").GetComponent<UIManager>();
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _uiManager.ShowChoice(collectible);
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _uiManager.HideChoice();
        }
    }
}