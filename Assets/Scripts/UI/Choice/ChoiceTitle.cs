using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChoiceTitle : MonoBehaviour
{
    private TextMeshProUGUI _textMeshProUGUI;

    void Awake()
    {
        _textMeshProUGUI = GetComponent<TextMeshProUGUI>();
    }

    public void SetTitle(string text)
    {
        _textMeshProUGUI.text = text;
    }
}
