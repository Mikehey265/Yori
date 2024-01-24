using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomStartTile : MonoBehaviour
{
    public GameEventInt roomStarted;
    private Vector3 playerPos;
    private bool eventFired = false;
    public int roomToStart;
    private bool roomAlreadyStarted;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (CheckPlayerPosition())
        {
            if (!eventFired)
            {
                roomStarted.EventRaised(roomToStart);
                eventFired = true;
                Debug.Log("Room Finish event fired");
            }
        }
    }

    private bool CheckPlayerPosition()
    {
        var position = MovePlayer.Instance.gameObject.transform.position;
        playerPos = new Vector3(position.x, 0, position.z);
        if (playerPos == gameObject.transform.position)
        {
            return true;
        }
        
        return false;
    }
}
