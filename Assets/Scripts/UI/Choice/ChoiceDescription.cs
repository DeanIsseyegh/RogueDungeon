using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChoiceDescription : MonoBehaviour
{
    private TextMeshProUGUI _textMeshProUGUI;

    // Start is called before the first frame update
    void Awake()
    {
        _textMeshProUGUI = GetComponent<TextMeshProUGUI>();
    }

    public void SetDescription(string description)
    {
        _textMeshProUGUI.text = description;
    }
}
