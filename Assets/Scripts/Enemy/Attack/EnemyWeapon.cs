using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    public bool IsActive { private get; set; }
    
    private void OnTriggerEnter(Collider other)
    {
        if (IsActive && other.CompareTag("Player"))
        {
            PlayerHealth playerControl = other.gameObject.GetComponent<PlayerHealth>();
            playerControl.TakeDamage();
        }
    }
}
