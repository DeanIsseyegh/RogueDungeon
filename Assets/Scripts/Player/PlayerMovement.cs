using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement
{
    private NavMeshAgent _playerNavMeshAgent;
    private GameObject clickIndicator;
    private int _rotateSpeedOnClick;

    public PlayerMovement(NavMeshAgent playerNavMeshAgent, GameObject clickIndicator)
    {
        _playerNavMeshAgent = playerNavMeshAgent;
        this.clickIndicator = clickIndicator;
    }

    public void MovePlayer(Vector3 mousePosition)
    {
        var direction = (mousePosition - _playerNavMeshAgent.transform.position).normalized;
        var lookRotation = Quaternion.LookRotation(direction);
        _rotateSpeedOnClick = 20;
        _playerNavMeshAgent.transform.rotation = Quaternion.Lerp(_playerNavMeshAgent.transform.rotation, lookRotation, Time.deltaTime * _rotateSpeedOnClick);
        _playerNavMeshAgent.SetDestination(mousePosition);
        clickIndicator.transform.position = mousePosition;
    }
}
