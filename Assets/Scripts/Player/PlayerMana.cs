using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMana : MonoBehaviour
{
    [SerializeField] private float maxMana = 100;
    [SerializeField] private float manaRegenRate = 5;
    public float CurrentMana { get; private set; }
    private UIManager _uiManager;

    private void Start()
    {
        CurrentMana = maxMana;
        _uiManager = GameObject.FindWithTag("UIManager").GetComponent<UIManager>();
        
        _uiManager.SetMaxMana(maxMana);
        _uiManager.SetCurrentMana(CurrentMana);
    }

    private void Update()
    {
        SetMana(CurrentMana + manaRegenRate * Time.deltaTime);
        if (CurrentMana > maxMana) SetMana(maxMana);
    }

    public void UseMana(float manaToUse)
    {
        SetMana(CurrentMana - manaToUse);
    }

    private void SetMana(float value)
    {
        CurrentMana = value;
        _uiManager.SetCurrentMana(CurrentMana);
    }

    public void AddMaxMana(float manaToAdd)
    {
        maxMana += manaToAdd;
        CurrentMana += manaToAdd;
        if (CurrentMana > maxMana)
        {
            CurrentMana = maxMana;
        }
        _uiManager.SetMaxMana(maxMana);
        _uiManager.SetCurrentMana(CurrentMana);
    }
}
