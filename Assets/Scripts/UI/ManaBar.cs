using System;
using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
{
    private Image _manaBarImage;

    private void Awake()
    {
        _manaBarImage = GameObject.FindWithTag("ManaBar").GetComponent<Image>();
    }

    public Image ManaBarImage
    {
        set => _manaBarImage = value;
    }
    public float MaxMana { private get; set; }

    public float CurrentMana
    {
        set => _manaBarImage.fillAmount = value / MaxMana;
    }
}