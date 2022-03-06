using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceIcon : MonoBehaviour
{
    private Image _image;

    // Start is called before the first frame update
    void Awake()
    {
        _image = GetComponent<Image>();
    }

    public void SetIcon(Sprite sprite)
    {
        _image.sprite = sprite;
    }
}
