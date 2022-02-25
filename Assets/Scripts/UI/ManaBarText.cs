using TMPro;
using UnityEngine;

public class ManaBarText : MonoBehaviour
{
    private TextMeshProUGUI _manaBarText;

    private void Awake()
    {
        _manaBarText = GameObject.FindWithTag("ManaBarText").GetComponent<TextMeshProUGUI>();
    }

    public float MaxMana { private get; set; }

    public float CurrentMana
    {
        set => _manaBarText.text = $"{(int) value}/{(int) MaxMana}";
    }
    
}