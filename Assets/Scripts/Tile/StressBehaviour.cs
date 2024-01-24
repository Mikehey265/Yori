using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StressBehaviour : MonoBehaviour
{
    public int stressAmount;
    public bool isMarked;
    public int posX;
    public int posY;

    private void Start()
    {

        posX = (int)transform.position.x;
        posY = (int)transform.position.z;
    }
}
