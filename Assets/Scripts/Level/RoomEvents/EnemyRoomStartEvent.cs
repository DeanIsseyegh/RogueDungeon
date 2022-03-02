using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyRoomStartEvent : MonoBehaviour
{

    private RandomGameObjGenerator _enemyGenerator;
    public List<Vector3> EnemyPositions  { private get; set; }

    private bool isEnemiesSpawned;
    
    private List<GameObject> _createdEnemies;

    protected virtual void Awake()
    {
        _enemyGenerator = GameObject.FindWithTag("RandomEnemyGenerator").GetComponent<RandomGameObjGenerator>();
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            EnemyPositions.ForEach(enemyPosition =>
            {
                var createdEnemy = _enemyGenerator.Generate(enemyPosition);
                _createdEnemies.Add(createdEnemy);
            });
            isEnemiesSpawned = true;
            GetComponent<BoxCollider>().enabled = false;
        }
    }

    public bool HasEventFinished()
    {
        return isEnemiesSpawned && _createdEnemies.All(it => it == null);
    }
}
