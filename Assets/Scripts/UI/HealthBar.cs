using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image healthBarImage;

    public Image HealthBarImage
    {
        set => healthBarImage = value;
    }
    public float MaxHealth { private get; set; }

    public float CurrentHealth
    {
        set => healthBarImage.fillAmount = value / MaxHealth;
    }
}