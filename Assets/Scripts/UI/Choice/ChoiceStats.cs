using TMPro;
using UnityEngine;

public class ChoiceStats : MonoBehaviour
{
    private TextMeshProUGUI _textMeshProUGUI;

    void Awake()
    {
        _textMeshProUGUI = GetComponent<TextMeshProUGUI>();
    }

    public void SetStats(string text)
    {
        _textMeshProUGUI.text = text;
    }

}
