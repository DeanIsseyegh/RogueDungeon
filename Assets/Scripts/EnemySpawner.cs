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
        createdEnemy.GetComponent<NavMeshAgent>().enabled = false;
        createdEnemy.GetComponent<Collider>().enabled = false;
        createdEnemy.GetComponent<EnemyAI>().enabled = false;
        GameObject healthBarCanvasObj = createdEnemy.GetComponentInChildren<Canvas>().gameObject;
        healthBarCanvasObj.SetActive(false);
        List<GameObject> createdEffects = spawnEffects.Select(effect => Instantiate(effect, position, Quaternion.identity)).ToList();
        StartCoroutine(StartSpawning(_desiredPosition, createdEnemy, healthBarCanvasObj, createdEffects));
        return createdEnemy;
    }
    
    private IEnumerator StartSpawning(Vector3 desiredPosition, GameObject createdEnemy, GameObject healthBarCanvasObj, List<GameObject> createdEffects)
    {
        while (true)
        {
            Debug.Log("Starting Spawn");
            createdEnemy.transform.position =
                Vector3.MoveTowards(createdEnemy.transform.position, desiredPosition, 1f * Time.deltaTime);

            if (Vector3.Distance(createdEnemy.transform.position, desiredPosition) < 0.1f)
            {
                createdEnemy.GetComponent<NavMeshAgent>().enabled = true;
                createdEnemy.GetComponent<Collider>().enabled = true;
                createdEnemy.GetComponent<EnemyAI>().enabled = true;
                healthBarCanvasObj.SetActive(true);
                createdEffects.ForEach(Destroy);
                Debug.Log("Ending the Spawn");
                yield break;
            }

            yield return new WaitForEndOfFrame();
            Debug.Log("Waited for End Of Spawn");        
        }
    }

    

}
