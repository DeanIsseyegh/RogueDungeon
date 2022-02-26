using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Sprite defaultSpellIcon;
    
    private HealthBar _healthBar;
    private HealthBarText _healthBarText;
    private ManaBar _manaBar;
    private ManaBarText _manaBarText;
    private List<SpellIcon> _spellIcons;

    private void Awake()
    {
        _healthBar = GetComponent<HealthBar>();
        _healthBarText = GetComponent<HealthBarText>();

        _manaBar = GetComponent<ManaBar>();
        _manaBarText = GetComponent<ManaBarText>();

        List<SpellIcon> spellIcons = GetComponents<SpellIcon>().ToList();
        _spellIcons = spellIcons.OrderBy(it => it.SpellIconPosition).ToList();
    }

    private void Start()
    {
        _spellIcons.ForEach(it => it.UpdateIcon(defaultSpellIcon));
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

    public void UpdateSpellIcon(Sprite newIcon, int spellPos)
    {
        _spellIcons[spellPos].UpdateIcon(newIcon);
    }
    
    public void UpdateSpellCooldown(float cooldown, float timeLeft, int spellPos)
    {
        _spellIcons[spellPos].UpdateCooldown(cooldown, timeLeft);
    }


}
