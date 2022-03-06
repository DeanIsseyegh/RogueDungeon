using UnityEngine;

public class OnPickUp : MonoBehaviour
{
    [field: SerializeField] public Collectible Collectible { get; private set; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInventory playerInventory = other.GetComponent<PlayerInventory>();
            playerInventory.PickupItem(Collectible);
            Destroy(this.gameObject);
        }
    }
}