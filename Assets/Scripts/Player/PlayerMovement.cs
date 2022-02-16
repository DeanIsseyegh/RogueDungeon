using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement
{
    private Camera camera;
    private NavMeshAgent _playerNavMeshAgent;
    private GameObject clickIndicator;

    public PlayerMovement(Camera camera, NavMeshAgent playerNavMeshAgent, GameObject clickIndicator)
    {
        this.camera = camera;
        _playerNavMeshAgent = playerNavMeshAgent;
        this.clickIndicator = clickIndicator;
    }

    public void MovePlayer(Vector3 mousePosition)
    {
        RaycastHit hit;
        Ray screenPointToRay = camera.ScreenPointToRay(mousePosition);
        bool isRaycast = Physics.Raycast(screenPointToRay, out hit, 100);
        if (isRaycast)
        {
            _playerNavMeshAgent.SetDestination(hit.point);
            clickIndicator.transform.position = hit.point;
        }
    }
}
