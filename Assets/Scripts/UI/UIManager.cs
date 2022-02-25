using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private HealthBar _healthBar;
    private HealthBarText _healthBarText;
    private ManaBar _manaBar;
    private ManaBarText _manaBarText;

    private void Start()
    {
        _healthBar = GetComponent<HealthBar>();
        _healthBarText = GetComponent<HealthBarText>();
        
        _manaBar = GetComponent<ManaBar>();
        _manaBarText = GetComponent<ManaBarText>();
    }
    
    public void SetMaxHealth(float value)
    {
        _healthBar.MaxHealth = value;
        _healthBarText.MaxHealth = value;
    }
    
    public void SetMaxMana(float value)
    {
        _manaBar.MaxMana = value;
        _manaBarText.MaxMana = value;
    }

    public void SetCurrentHealth(float value)
    {
        _healthBar.CurrentHealth = value;
        _healthBarText.CurrentHealth = value;
    }

    public void SetCurrentMana(float value)
    {
        _manaBar.CurrentMana = value;
        _manaBarText.CurrentMana = value;
    }

 
}
