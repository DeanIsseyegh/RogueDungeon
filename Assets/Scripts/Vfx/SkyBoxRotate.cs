using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyBoxRotate : MonoBehaviour
{
    private static readonly int Rotation = Shader.PropertyToID("_Rotation");

    [SerializeField] private float rotateSpeed = 0.4f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RenderSettings.skybox.SetFloat(Rotation, Time.time * rotateSpeed);
        DynamicGI.UpdateEnvironment();
    }
}
