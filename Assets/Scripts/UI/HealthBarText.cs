using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthBarText : MonoBehaviour
{
    private TextMeshProUGUI _healthBarText;

    private void Awake()
    {
        _healthBarText = GameObject.FindWithTag("HealthBarText").GetComponent<TextMeshProUGUI>();
    }

    public float MaxHealth { private get; set; }

    public float CurrentHealth
    {
        set => _healthBarText.text = $"{(int) value}/{(int) MaxHealth}";
    }
}
