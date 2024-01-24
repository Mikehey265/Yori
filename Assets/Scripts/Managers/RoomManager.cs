using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance { get; private set; }
    
    public List<GameObject> enemies;
    public List<GameObject> startTiles;
    public List<Transform> cameraCenterPoints;
    public GameEventInt roomFinished;
    public Camera playerCamera;

    private Vector3 playerPosition;
    
    public int currentRoom;
    public int finalRoomIndex;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (SavingSystem.Instance.IsCurrentRoomSaved())
        {
            currentRoom = SavingSystem.Instance.GetCurrentRoom();
        }
        else
        {
            currentRoom = 0;
        }
        MoveCamera(currentRoom);
        ActivateEnemy();
        InGameUI.Instance.IsBagPickedUp();
        InGameUI.Instance.UpdateItemsUI();
    }



    private void OnEnable()
    {
        roomFinished.onEventRaised += RoomFinished;
    }

    private void OnDisable()
    {
        roomFinished.onEventRaised -= RoomFinished;
    }

    private void RoomFinished(int roomNr)
    {
        MovePlayerToNewRoom(roomNr);
        currentRoom++;
        MoveCamera(currentRoom);
        if (currentRoom == finalRoomIndex)
        {
            // InGameUI.Instance.ActivateWinPanel();
            StartCoroutine(OpenWinPanel());
            return;
        }
        ActivateEnemy();
        ClearInventories();
        
    }
    
    private void MoveCamera(int roomNr)
    {
        playerCamera.transform.position = cameraCenterPoints[roomNr].position;
    }

    private void MovePlayerToNewRoom(int roomNr)
    {
        var position = startTiles[roomNr].transform.position;
        playerPosition = new Vector3(position.x, 1, position.z);
        MovePlayer.Instance.SetCurrentPlayerPosition(playerPosition);
    }

    private void ActivateEnemy()
    {
        foreach (GameObject enemy in enemies)
        {
            if (enemy.GetComponent<EnemyMovement>().activateInRoom == currentRoom)
            {
                enemy.SetActive(true);
            }
            else
            {
                enemy.SetActive(false);
            }
        }
    }

    private void ClearInventories()
    {
        InventoryManager.Instance.items.Clear();
        InventoryManager.Instance.inventory.itemsInventory.Clear();
        InGameUI.Instance.ClearItemsUI();
    }

    private IEnumerator OpenWinPanel()
    {
        yield return new WaitForSeconds(4);
        InGameUI.Instance.ActivateWinPanel();
    }
    
}
