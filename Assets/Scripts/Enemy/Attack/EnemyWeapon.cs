using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    [SerializeField] private WeaponData weaponData;
    private DamagingAttack _damagingAttack;
    public bool IsActive { private get; set; }

    private void Awake()
    {
        _damagingAttack = gameObject.AddComponent<DamagingAttack>();
        _damagingAttack.Damage = weaponData.damage;
        _damagingAttack.TriggersOnTag = "Player";
        _damagingAttack.enabled = false;
    }

    private void Update()
    {
        _damagingAttack.enabled = IsActive;
    }

}
