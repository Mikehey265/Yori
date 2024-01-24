using System.Collections.Generic;
using UnityEngine;

public class StressManager : MonoBehaviour
{
    public static StressManager Instance { get; private set; }
    
    [SerializeField] private int maxStressPoints;
    private Vector3 playerPos = new Vector3();
    public GameEventVoid setVisionTilesBehaviour;
    [SerializeField] private int currentStressPoints;
    private TileReferences tileReferences;
    private List<StressBehaviour> stressTiles = new List<StressBehaviour>();
    [SerializeField] private AudioSource gaspSound;
    

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        tileReferences = TileReferences.Instance;
        if (SavingSystem.Instance.ArePlayerStatsSaved())
        {
            currentStressPoints = SavingSystem.Instance.GetCurrentPlayerStress();
            InGameUI.Instance.UpdateStressBar(currentStressPoints, maxStressPoints);
        }
        else
        {
            currentStressPoints = 0;
            InGameUI.Instance.UpdateStressBar(currentStressPoints, maxStressPoints);
        }
    }

    private void OnEnable()
    {
        setVisionTilesBehaviour.onEventRaised += CheckPlayerPosition;
    }

    private void OnDisable()
    {
        setVisionTilesBehaviour.onEventRaised -= CheckPlayerPosition;
    }

    public void AddStressPoints(int stressAmount)
    {
        if (RoomManager.Instance.currentRoom == 0) return;
        if (currentStressPoints < maxStressPoints)
        {
            currentStressPoints += stressAmount;
            InGameUI.Instance.UpdateStressBar(currentStressPoints, maxStressPoints);
        }
        if (currentStressPoints >= maxStressPoints)
        {
            InGameUI.Instance.ActivateLostGamePanel();
        }
    }

    public void RemoveStressPoint()
    {
        if (currentStressPoints > 0)
        {
            currentStressPoints--;   
            InGameUI.Instance.UpdateStressBar(currentStressPoints, maxStressPoints);   
        }
    }

    public int GetCurrentStressPoints()
    {
        return currentStressPoints;
    }

    private void GetCurrentlyMarkedTiles()
    {
        foreach (GameObject obj in tileReferences.tileReferencesArray)
        {
            StressBehaviour stressBehaviour = obj.GetComponent<StressBehaviour>();
            if (stressBehaviour.isMarked)
            {
                stressTiles.Add(stressBehaviour);
            }
        }
    }
    
    public void CheckPlayerPosition()
    {
        GetCurrentlyMarkedTiles();
        var position = MovePlayer.Instance.gameObject.transform.position;
        playerPos = new Vector3(position.x, 0, position.z);
        for (int i = 0; i < stressTiles.Count; i++)
        {
            if (playerPos == stressTiles[i].gameObject.transform.position)
            {
                AddStressPoints(stressTiles[i].stressAmount);
                if (gaspSound != null)
                {
                    gaspSound.Play();
                }
                
            }
        }

        stressTiles.Clear();
    }
}
