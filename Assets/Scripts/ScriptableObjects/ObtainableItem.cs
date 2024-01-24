using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Items/Obtainable Item")]
public class ObtainableItem : ScriptableObject
{
    public ObtainableItem item;
    public string nameString;
    public GameObject obj;
    public bool isObtained = false;
    public bool isObtainable = false;
    public Sprite sprite;
    public Image image;
    public bool bagInitializer;

}
