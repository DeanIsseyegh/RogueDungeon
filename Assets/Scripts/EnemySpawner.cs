using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] spawnEffects; 
    [SerializeField] private Vector3 spawnStartOffset = new Vector3(0, -3, 0);

    public GameObject Spawn(Vector3 position, GameObject enemyPrefab, Quaternion quaternion)
    {
        Vector3 _desiredPosition = position;
        Vector3 posToStartSpawn = position + spawnStartOffset;
        GameObject createdEnemy = Instantiate(enemyPrefab, posToStartSpawn, quaternion);
        GameObject healthBarCanvasObj = createdEnemy.GetComponentInChildren<Canvas>().gameObject;
        SetEnabledCoreComponents(createdEnemy, healthBarCanvasObj, false);
        List<GameObject> createdEffects = spawnEffects.Select(effect => Instantiate(effect, position, Quaternion.identity)).ToList();
        StartCoroutine(StartSpawning(_desiredPosition, createdEnemy, healthBarCanvasObj, createdEffects));
        return createdEnemy;
    }

    private IEnumerator StartSpawning(Vector3 desiredPosition, GameObject createdEnemy, GameObject healthBarCanvasObj, List<GameObject> createdEffects)
    {
        while (true)
        {
            createdEnemy.transform.position =
                Vector3.MoveTowards(createdEnemy.transform.position, desiredPosition, 1f * Time.deltaTime);

            if (Vector3.Distance(createdEnemy.transform.position, desiredPosition) < 0.1f)
            {
                SetEnabledCoreComponents(createdEnemy,  healthBarCanvasObj, true);
                createdEffects.ForEach(Destroy);
                yield break;
            }

            yield return new WaitForEndOfFrame();
        }
    }

    private static void SetEnabledCoreComponents(GameObject createdEnemy, GameObject healthBarCanvasObj,
        bool isEnabled)
    {
        createdEnemy.GetComponent<NavMeshAgent>().enabled = isEnabled;
        createdEnemy.GetComponent<Collider>().enabled = isEnabled;
        createdEnemy.GetComponent<EnemyAI>().enabled = isEnabled;
        healthBarCanvasObj.SetActive(isEnabled);
    }

}
