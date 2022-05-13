using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class RotateCameraAroundTarget : MonoBehaviour
{
    [SerializeField] private GameObject cinemachineCameraTarget;
    [SerializeField] private float rotateSpeed = 20f;
    
    [SerializeField] private bool shouldSetInitialCameraRotation = false;
    [SerializeField] private Vector3 initialCameraRotationTarget = Vector3.zero;
    [SerializeField] private float initialCameraRotationSpeed = 0.1f;

    private Quaternion initialTargetRotation;
    private Quaternion initialStartRotation;
    private float lerpT = 0;

    private void Start()
    {
        if (shouldSetInitialCameraRotation)
        {
            initialStartRotation = cinemachineCameraTarget.transform.rotation;
            initialTargetRotation = Quaternion.Euler(initialCameraRotationTarget);
        }
        else
        {
            initialTargetRotation = cinemachineCameraTarget.transform.rotation;
        }
    }

    private void Update()
    {
        if (lerpT < 1)
        {
            Quaternion lerpRotation = Quaternion.Lerp(initialStartRotation, initialTargetRotation, lerpT);
            cinemachineCameraTarget.transform.rotation = lerpRotation;
            lerpT +=  Time.deltaTime * initialCameraRotationSpeed;
        }
        else
        {
            cinemachineCameraTarget.transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime, Space.World);
        }
    }
}