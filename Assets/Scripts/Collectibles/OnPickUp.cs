using UnityEngine;

public class OnPickUp : MonoBehaviour
{
    [SerializeField] private Collectible collectible;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInventory playerInventory = other.GetComponent<PlayerInventory>();
            playerInventory.PickupItem(collectible);
            Destroy(this.gameObject);   
        }
    }
}
