using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider healthBarSlider;

    public Slider HealthBarImage
    {
        set => healthBarSlider = value;
        get => healthBarSlider;
    }
    public float MaxHealth { private get; set; }

    public float CurrentHealth
    {
        set => healthBarSlider.value = value / MaxHealth;
    }
}