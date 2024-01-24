using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileReferences : MonoBehaviour
{
    public static TileReferences Instance { get; private set; }
    public List<GameObject> TileReferencesList;
    public GameObject[] tileReferencesArray;

    private void Awake()
    {
        Instance = this;
        TileReferencesList = new List<GameObject>();
        tileReferencesArray = GameObject.FindGameObjectsWithTag("Tile");

    }
    
}
