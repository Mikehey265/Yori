using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    public static InGameUI Instance { get; private set; }
    
    [Header("UI Panels")]
    [SerializeField] private GameObject overlayUI;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject lostPanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject stressMinigameKey;
    
    [Header("Loading Screen")]
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private float loadingScreenLength;
    
    [Header("Other Settings")]
    [SerializeField] private TextMeshProUGUI stepsTakenText;
    [SerializeField] private TextMeshProUGUI stepsWinPanelText;
    [SerializeField] private Image stressBar;
    [SerializeField] private ObtainableItem inventoryInitialiser;
    [SerializeField] private List<GameObject> itemsGameObjects;
    
    private bool isGamePaused;
    private bool isGameLoading;

    private void Awake()
    {
        Instance = this;

        StartCoroutine(StartLoadingScreen(loadingScreenLength));
        
        stepsTakenText.text = "0";
        overlayUI.SetActive(true);
        pauseMenu.SetActive(false);
        lostPanel.SetActive(false);
        winPanel.SetActive(false);
        stressMinigameKey.SetActive(false);
        Time.timeScale = 1;
        isGamePaused = false;
        ClearItemsUI();
    }

    private void Start()
    {
        if (RoomManager.Instance.currentRoom == 0)
        {
            inventoryPanel.SetActive(false);
        }
    }


    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isGamePaused = true;
    }
    
    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isGamePaused = false;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        PlayerPrefs.DeleteAll();
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isGamePaused = false;
    }

    public void SaveGame()
    {
        Vector3 player = MovePlayer.Instance.GetCurrentPlayerPosition();
        Vector3 enemy = EnemyMovement.Instance.GetCurrentEnemyPosition();
        
        SavingSystem.Instance.SavePlayerStats(player.x, player.z, MovePlayer.Instance.GetClicksAmount(), StressManager.Instance.GetCurrentStressPoints());
        SavingSystem.Instance.SaveEnemyPosition(enemy.x, enemy.z, EnemyMovement.Instance.GetCurrentPatrolPoint());
        SavingSystem.Instance.SaveCurrentRoom(RoomManager.Instance.currentRoom);
        //SaveInventory.Instance.Save(InventoryManager.Instance.inventory);
        SceneManager.LoadScene("MainMenuScene");
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isGamePaused = false;
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
        PlayerPrefs.DeleteAll();
        lostPanel.SetActive(false);
        winPanel.SetActive(false);
        Time.timeScale = 1f;
        isGamePaused = false;
    }

    public void ActivateLostGamePanel()
    {
        lostPanel.SetActive(true);
        overlayUI.SetActive(false);
        Time.timeScale = 1f;
        isGamePaused = true;
    }

    public void ActivateWinPanel()
    {
        winPanel.SetActive(true);
        overlayUI.SetActive(false);
        Time.timeScale = 1f;
        isGamePaused = true;
        stepsWinPanelText.text = stepsTakenText.text;
    }

    public void BagInitialize()
    {
        inventoryPanel.SetActive(true);
    }

    public void IsBagPickedUp()
    {
        if (RoomManager.Instance.currentRoom == 0)
        {
            inventoryPanel.SetActive(false);
        }
        else
        {
            inventoryPanel.SetActive(true);
        }
    }
    
    public void UpdateItemsUI()
    {
        ClearItemsUI();
        if (InventoryManager.Instance.inventory.itemsInventory.Contains(inventoryInitialiser) && RoomManager.Instance.currentRoom == 0)
        {
            BagInitialize();
            return;
        }
        for (int i = 0; i < InventoryManager.Instance.inventory.itemsInventory.Count; i++)
        {
            itemsGameObjects[i].GetComponent<ItemUI>().item = InventoryManager.Instance.inventory.itemsInventory[i];
            itemsGameObjects[i].GetComponent<ItemUI>().ChangeSprite();
            itemsGameObjects[i].SetActive(true);
        }
    }

    public void ClearItemsUI()
    {
        for (int i = 0; i < itemsGameObjects.Count; i++)
        {
            itemsGameObjects[i].GetComponent<ItemUI>().item = null;
            itemsGameObjects[i].SetActive(false);
        }
    }

    public void UpdateStressBar(float currentStress, float maxStress)
    {
        stressBar.fillAmount = currentStress / maxStress;

        if (StressManager.Instance.GetCurrentStressPoints() > 0)
        {
            stressMinigameKey.SetActive(true);
        }
        else
        {
            stressMinigameKey.SetActive(false);
        }
    }

    public void UpdateStepsCounterText(int stepsAmount)
    {
        stepsTakenText.text = stepsAmount.ToString();
        stepsWinPanelText.text = stepsAmount.ToString();
    }

    public bool GetIsGamePaused()
    {
        return isGamePaused;
    }

    public bool GetIsGameLoading()
    {
        return isGameLoading;
    }

    private IEnumerator StartLoadingScreen(float loadingScreenLength)
    {
        isGameLoading = true;
        loadingScreen.SetActive(true);
        yield return new WaitForSeconds(loadingScreenLength);
        loadingScreen.SetActive(false);
        isGameLoading = false;
    }
}
