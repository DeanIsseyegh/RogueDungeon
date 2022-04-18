using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BalloonManager : MonoBehaviour
{

    private readonly List<GameObject> balloons = new List<GameObject>();
    private bool _isAllBalloonsDestroyed;

    public void StartGame()
    {
        balloons.ForEach( it => it.GetComponent<MoveRight>().enabled = true );
    }
    
    void Update()
    {
        if (_isAllBalloonsDestroyed) return;

        if (balloons.All(it => it == null))
        {
            _isAllBalloonsDestroyed = true;
        }
    }

    public void Register(GameObject balloon)
    {
        balloons.Add(balloon);
    }

    public bool AreAllBalloonsDestroyed()
    {
        return _isAllBalloonsDestroyed;
    }
    
}
