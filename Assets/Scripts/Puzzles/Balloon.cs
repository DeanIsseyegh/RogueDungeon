using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Balloon : MonoBehaviour
{
    [FormerlySerializedAs("collidesWith")] [SerializeField] private string destroyedBy;
    [SerializeField] private List<GameObject> destructionEffects;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Balloon trigger on " + other.tag);
        Debug.Log("collides with " + destroyedBy);
        if (other.CompareTag(destroyedBy))
        {
            Debug.Log("Created effects for baloon");
            destructionEffects.ForEach(effect =>
            {
                GameObject createdEffect = Instantiate(effect, transform.position, Quaternion.identity);
                Destroy(createdEffect, 4f);
            });
            Destroy(this.gameObject);
        }
    }

}
