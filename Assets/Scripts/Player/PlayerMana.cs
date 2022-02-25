using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMana : MonoBehaviour
{
    [SerializeField] private float maxMana = 100;
    private float _currentMana;
    private ManaBar _manaBar;
    private ManaBarText _manaBarText;

    private void Start()
    {
        _currentMana = maxMana;
        _manaBar = GetComponent<ManaBar>();
        _manaBarText = GetComponent<ManaBarText>();

        _manaBar.MaxMana = maxMana;
        _manaBar.CurrentMana = _currentMana;
        _manaBarText.MaxMana = maxMana;
        _manaBarText.CurrentMana = _currentMana;
    }

    public void UseMana(float manaToUse)
    {
        _currentMana -= manaToUse;
        _manaBar.CurrentMana = _currentMana;
        _manaBarText.CurrentMana = _currentMana;
    }
}
