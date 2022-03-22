using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Open : MonoBehaviour
{
    private void Update()
    {
        transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(0, 90f, 0), Time.deltaTime);
    }

}
