using UnityEngine;

public class Open : MonoBehaviour
{
    private void Update()
    {
        transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(0, 90f, 0), Time.deltaTime);
    }

}
