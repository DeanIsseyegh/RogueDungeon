using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Image healthBarImage;

    public float MaxHealth { private get; set; }

    public float CurrentHealth
    {
        set
        {
            healthBarImage.fillAmount = value / MaxHealth;
        }
    }
}