using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyRoomStartEvent : MonoBehaviour
{
    private RandomGameObjGenerator _enemyGenerator;
    public List<Vector3> EnemyPositions { private get; set; }

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
            _createdEnemies = EnemyPositions.Select(pos => _enemyGenerator.Generate(pos)).ToList();
            isEnemiesSpawned = true;
            GetComponent<BoxCollider>().enabled = false;
        }
    }

    public bool HasEventFinished()
    {
        return isEnemiesSpawned && _createdEnemies.All(it => it == null);
    }
}