using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellIcon : MonoBehaviour
{
    [SerializeField] [Range(1, 4)] private int spellIconPosition;
    public int SpellIconPosition => spellIconPosition;
    private Image _iconImage;
    private Image _iconCooldownImage;

    private void Awake()
    {
        const string iconTagPrefix = "SpellIcon";
        ;
        var iconTag = iconTagPrefix + SpellIconPosition;
        _iconImage = GameObject.FindWithTag(iconTag).GetComponent<Image>();
        var iconCooldownTag = iconTag + "Cooldown";
        _iconCooldownImage = GameObject.FindWithTag(iconCooldownTag).GetComponent<Image>();
    }

    public void UpdateIcon(Sprite newIcon)
    {
        _iconImage.sprite = newIcon;
    }

    public void UpdateCooldown(float cooldown, float timeLeft)
    {
        if (timeLeft <= 0)
        {
            _iconCooldownImage.fillAmount = 0;
        }
        else
        {
            _iconCooldownImage.fillAmount = timeLeft / cooldown;
        }
    }
}