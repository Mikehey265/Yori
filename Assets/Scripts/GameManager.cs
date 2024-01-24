using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    public GameObject miniGamePrefab;
    private ReduceStressGameScript miniGameRef;
    
    private bool isReduceStressMiniGameActive = false;
    private bool isPlayerInFirstRoom;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        miniGameRef = miniGamePrefab.GetComponent<ReduceStressGameScript>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (MovePlayer.Instance.GetIsPlayerMoving())
            {
                Debug.LogWarning("Can't play game while moving");
            }
            else if(!isReduceStressMiniGameActive && StressManager.Instance.GetCurrentStressPoints() > 0 
                        && RoomManager.Instance.currentRoom != 3 && !InGameUI.Instance.GetIsGamePaused())
            {
                OpenMiniGame();
            }
        }else if (!miniGameRef.GetIsGameActive())
        {
            CloseMiniGame();
            //Debug.Log($"Current stress:{StressManager.Instance.GetCurrentStressPoints()}");
        }
    }
    
    public void OpenMiniGame()
    {
        Debug.Log($"Current stress:{StressManager.Instance.GetCurrentStressPoints()}");
        if (miniGamePrefab != null)
        {
            miniGamePrefab.SetActive(true);
            
            StartMiniGame();  // Start the mini game here
            
            isReduceStressMiniGameActive = true;
        }
        else
        {
            Debug.LogError("Mini game prefab not assigned!");
        }
    }

    public void CloseMiniGame()
    {
        if (miniGamePrefab != null)
        {
            miniGamePrefab.SetActive(false); 
            isReduceStressMiniGameActive = false;
        }
    }
    
    private void StartMiniGame()
    {
        // Reset and start the mini-game
        miniGameRef.ResetGame();
    }
    
    public bool ReturnMiniGameActive()
    {
        return isReduceStressMiniGameActive;
    }
}



