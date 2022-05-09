using UnityEngine;

public class HomingAttack : MonoBehaviour
{
    private GameObject _player;
    public float RotationSpeed { set; private get; }
    public string HomingTargetTag { set; private get; }
    public Vector3 TargetOffset { set; private get; }

    private bool _hasReachedDest;

    private void Awake()
    {
        _player = GameObject.FindWithTag("Player");
    }

    private void Update()
    {
        if (_hasReachedDest) return;
        
        Vector3 dirToRotateTo = (_player.transform.position + TargetOffset) - this.transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dirToRotateTo);
        transform.rotation = Quaternion.RotateTowards(this.transform.rotation, lookRotation, RotationSpeed);
        
        // float distance = Vector3.Distance(_player.transform.position, this.transform.position);
        // if (distance < 1f)
        // {
        //     _hasReachedDest = true;
        // }
    }
}