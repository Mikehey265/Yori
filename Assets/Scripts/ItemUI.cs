using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    public ObtainableItem item;

    private Sprite sprite;
    private Image uiImage;

    public void ChangeSprite()
    {
        this.transform.GetComponent<UnityEngine.UI.Image>().sprite = item.sprite;
    }
}
