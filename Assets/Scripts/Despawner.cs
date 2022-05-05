using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Despawner : MonoBehaviour
{
    public void Despawn()
    {
        Destroy(this.gameObject);
    }
}
