using UnityEngine;
using UnityEngine.UI;

public class ItemIcons : MonoBehaviour
{
    [SerializeField] private GameObject firstItem;
    [SerializeField] private Vector3 nextIconOffset = new Vector3(65, 0, 0);

    private GameObject _prevItem;

    public void AddIcon(Sprite sprite)
    {
        if (_prevItem == null)
        {
            firstItem.SetActive(true);
            Add(firstItem, sprite);
        }
        else
        {
            GameObject nextItem = Instantiate(firstItem, firstItem.transform.position + nextIconOffset,
                Quaternion.identity, firstItem.transform.parent);
            Add(nextItem, sprite);
        }
    }

    private void Add(GameObject iconObj, Sprite sprite)
    {
        iconObj.name = sprite.name;
        GameObject itemIcon = iconObj.transform.GetChild(0).gameObject;
        Image image = itemIcon.GetComponent<Image>();
        image.sprite = sprite;
        _prevItem = itemIcon;
    }
}