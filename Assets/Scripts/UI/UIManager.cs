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
    private ItemIcons _itemIcons;
    private GameObject _choiceUi;

    private void Awake()
    {
        _healthBar = GetComponent<HealthBar>();
        _healthBarText = GetComponent<HealthBarText>();

        _manaBar = GetComponent<ManaBar>();
        _manaBarText = GetComponent<ManaBarText>();

        List<SpellIcon> spellIcons = GetComponents<SpellIcon>().ToList();
        _spellIcons = spellIcons.OrderBy(it => it.SpellIconPosition).ToList();
        
        _itemIcons = GetComponent<ItemIcons>();
        _choiceUi = GameObject.FindWithTag("SpellChoiceUI");
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

    public void AddItemIcon(Sprite newIcon)
    {
        _itemIcons.AddIcon(newIcon);
    }

    public void ShowChoice(Collectible collectible)
    {
        GameObject choice = _choiceUi.transform.GetChild(0).gameObject;
        choice.SetActive(true);
        choice.GetComponentInChildren<ChoiceIcon>().SetIcon(collectible.icon);
        choice.GetComponentInChildren<ChoiceTitle>().SetTitle(collectible.Info().collectibleName);
        choice.GetComponentInChildren<ChoiceDescription>().SetDescription(collectible.Info().description);
        choice.GetComponentInChildren<ChoiceStats>().SetStats(collectible.Info().stats);
    }

    public void HideChoices()
    {
        foreach (Transform child in _choiceUi.transform)
        {
            child.gameObject.SetActive(false);
        }
    }
}
