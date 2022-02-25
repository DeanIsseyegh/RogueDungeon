using UnityEngine;

public class MousePositionTracker : MonoBehaviour
{
    [SerializeField] private LayerMask floorLayerMask;
    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
    }

    public Vector3 MousePos()
    {
        Plane plane = new Plane(Vector3.up, new Vector3(0, -1.2f, 0));
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        plane.Raycast(ray, out var distanceToPlane);
        return ray.GetPoint(distanceToPlane);
    }
    
    public Vector3 MousePosOnFloor()
    {
        Ray screenPointToRay = _camera.ScreenPointToRay(Input.mousePosition);
        bool isRaycast = Physics.Raycast(screenPointToRay, out var hit, 100, floorLayerMask);
        return isRaycast ? hit.point : MousePos(); 
    }
    
}
