using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemIcons : MonoBehaviour
{
    [SerializeField] private GameObject[] items;
    private int itemIndex;

    private GameObject _prevItem;

    public void AddIcon(Sprite sprite)
    {
        GameObject item = items[itemIndex];
        item.SetActive(true);
        Add(item, sprite);
        itemIndex++;
    }

    private void Add(GameObject iconObj, Sprite sprite)
    {
        iconObj.name = sprite.name;
        GameObject itemIcon = iconObj.transform.GetChild(0).GetChild(1).gameObject;
        Image image = itemIcon.GetComponent<Image>();
        image.sprite = sprite;
        _prevItem = itemIcon;
    }
}