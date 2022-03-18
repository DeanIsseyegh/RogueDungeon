using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAiming : MonoBehaviour
{
    [SerializeField] private LayerMask mouseColliderLayerMask;
    public Vector3 MouseWorldPosition { get; private set; }

    void Update()
    {
        MouseWorldPosition = Vector3.zero;
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);

        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        MouseWorldPosition = ray.GetPoint(10);

        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999, mouseColliderLayerMask))
        {
            MouseWorldPosition = raycastHit.point;
        }
        else
        {
            MouseWorldPosition = ray.GetPoint(10);
        }
    }
}
