using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowcaseGenerator : MonoBehaviour
{

    [SerializeField] private List<GameObject> objsToShow;
    [SerializeField] private float showFrequency = 0.75f;
    private float _showTimer;
    private int _currentIdx;
    void Start()
    {
        _showTimer = showFrequency;
    }

    void Update()
    {
        _showTimer -= Time.deltaTime;
        if (_showTimer <= 0)
        {
            GameObject objToShow = objsToShow[_currentIdx];
            objToShow.SetActive(true);
            _showTimer = showFrequency;
            _currentIdx++;
        }
    }
}
