using UnityEngine;

public class RandomEnemyGenerator : RandomGameObjGenerator
{
    [SerializeField] private EnemySpawner enemySpawner;
    protected override GameObject CreateObj(Vector3 posToGenerate, GameObject objToCreate, Quaternion quaternion)
    {
        return enemySpawner.Spawn(posToGenerate, objToCreate, quaternion);
    }
}