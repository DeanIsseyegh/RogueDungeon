using UnityEngine;

public class HomingAttack : MonoBehaviour
{
    private GameObject _player;
    public float RotationSpeed { set; private get; }
    public GameObject Target { set; private get; }
    public Vector3 TargetOffset { set; private get; }

    private void Update()
    {
        if (Target == null) return;
        Vector3 dirToRotateTo = (Target.transform.position + TargetOffset) - this.transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dirToRotateTo);
        transform.rotation = Quaternion.RotateTowards(this.transform.rotation, lookRotation, RotationSpeed);
    }
}