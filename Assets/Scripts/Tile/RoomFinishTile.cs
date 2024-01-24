using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class RoomFinishTile : MonoBehaviour
{
    private Vector3 playerPos;
    public List<ObtainableItem> itemsRequired;
    private bool inventoryChecked;
    private int itemCollected;
    public GameEventInt roomFinished;
    public int roomNr;
    private bool endReached;
    [SerializeField] private TMP_Text nextRoomPrompt;
    

    private void Start()
    {
        endReached = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && endReached)
        {
            if (GameManager.Instance.ReturnMiniGameActive()) return;
            roomFinished.EventRaised(roomNr);
            endReached = false;
        }
    }

    private void FixedUpdate()
    {
        if (!CheckPlayerPosition()) return;
        if (!inventoryChecked)
        {
            CheckInventoryAgainstReqItems(InventoryManager.Instance.items);
        }
        
    }

    private void CheckInventoryAgainstReqItems(List<ObtainableItem> inventory)
    {
        itemCollected = 0;
        for (int i = 0; i < itemsRequired.Count; i++)
        {
            if(inventory.Contains(itemsRequired[i]))
            {
                Debug.Log("item " + itemsRequired[i].ToString() + " acquired");
                itemCollected++;
            }
        }

        if (itemCollected == itemsRequired.Count)
        {
            //player input is disabled
            endReached = true;
        }
        inventoryChecked = true;
    }
    
    private bool CheckPlayerPosition()
    {
        var position = MovePlayer.Instance.gameObject.transform.position;
        playerPos = new Vector3(position.x, 0, position.z);
        if (playerPos == gameObject.transform.position)
        {
            nextRoomPrompt.enabled = true;
            return true;
        }

        nextRoomPrompt.enabled = false;
        inventoryChecked = false;
        return false;
    }
}
