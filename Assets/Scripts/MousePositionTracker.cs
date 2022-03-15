using UnityEngine;
using UnityEngine.InputSystem;

public class MousePositionTracker : MonoBehaviour
{
    [SerializeField] private LayerMask floorLayerMask;
    private Camera _camera;
    private GameObject _player;
    private float _halfPlayerHeight;

    private void Awake()
    {
        _camera = Camera.main;
        _player = GameObject.FindWithTag("Player");
        _halfPlayerHeight = _player.GetComponent<BoxCollider>().size.y / 2;
    }

    public Vector3 MousePos()
    {
        float planeYPos = _player.transform.position.y + _halfPlayerHeight;
        Plane plane = new Plane(Vector3.up, new Vector3(0, planeYPos, 0));
        Ray ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());
        plane.Raycast(ray, out var distanceToPlane);
        return ray.GetPoint(distanceToPlane);
    }

    public Vector3 MousePosOnFloor()
    {
        Ray screenPointToRay = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());
        bool isRaycast = Physics.Raycast(screenPointToRay, out var hit, 100, floorLayerMask);
        return isRaycast ? hit.point : MousePos();
    }
}