using UnityEngine;

public class Weapon : MonoBehaviour
{

    [SerializeField] private PlayerControl _playerControl;
    
    public bool IsActive { private get; set; }

    private void OnTriggerEnter(Collider other)
    {
        if (IsActive && other.CompareTag("Player"))
        {
            _playerControl.TakeDamage();
        }
    }
    

}
