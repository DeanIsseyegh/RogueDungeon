using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Balloon : MonoBehaviour
{
    [SerializeField] private string destroyedBy;
    [SerializeField] private List<GameObject> destructionEffects;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(destroyedBy))
        {
            destructionEffects.ForEach(effect =>
            {
                GameObject createdEffect = Instantiate(effect, transform.position, Quaternion.identity);
                Destroy(createdEffect, 4f);
            });
            Destroy(this.gameObject);
        }
    }

}
