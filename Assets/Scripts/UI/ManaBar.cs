using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
{
    private Slider _manaBarSlider;

    private void Awake()
    {
        _manaBarSlider = GameObject.FindWithTag("ManaBar").GetComponent<Slider>();
    }

    public Slider ManaBarImage
    {
        set => _manaBarSlider = value;
    }
    public float MaxMana { private get; set; }

    public float CurrentMana
    {
        set => _manaBarSlider.value = value / MaxMana;
    }
}