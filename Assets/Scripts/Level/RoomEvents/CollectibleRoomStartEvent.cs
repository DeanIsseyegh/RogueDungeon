using UnityEngine;

public abstract class CollectibleRoomStartEvent : MonoBehaviour
{
    private Vector3 _collectibleHeightOffset;
    public RandomGameObjGenerator ItemGenerator { private get; set;  }
    public Vector3 MiddleOfRoomPos { private get; set; }

    public bool isItemCreated;
    
    private GameObject _createdItem;

    protected abstract string GetCollectibleGeneratorTag();
    protected virtual void Awake()
    {
        ItemGenerator = GameObject.FindWithTag(GetCollectibleGeneratorTag()).GetComponent<RandomGameObjGenerator>();
        _collectibleHeightOffset = Vector3.up * 1.5f;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _createdItem = ItemGenerator.Generate(MiddleOfRoomPos + _collectibleHeightOffset);
            isItemCreated = true;
            GetComponent<BoxCollider>().enabled = false;
        }
    }

    public bool HasEventFinished()
    {
        return isItemCreated && _createdItem == null;
    }
}
