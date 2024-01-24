using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class UIControler : MonoBehaviour
{

    [SerializeField] private GameObject MainMenu;
    [SerializeField] private GameObject SettingMenu;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject lostMenu;
    [SerializeField] private GameObject startGameBtn;
    [SerializeField] private GameObject Overlay;


    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0.0f;
    }
    public void Playe()
    {
        Time.timeScale = 1.0f;
        MainMenu.SetActive(false);
        pauseMenu.SetActive(false);

    }
    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1.0f;
        MainMenu.SetActive(false);
    }
   public void ResstartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1.0f;
    }
    public void ExitGame()
    {
        if (Application.isPlaying)
        {
         //   UnityEditor.EditorApplication.isPlaying = false;
        }
        else
        {
            Application.Quit();
        }
    }
    public void BackToMainMenu()
    {
        if (MainMenu || SettingMenu || lostMenu)
        {
            MainMenu.SetActive(true);
            pauseMenu.SetActive(false);
            lostMenu.SetActive(false);
            SettingMenu.SetActive(false);
        }
    }
    public void BackToPauseMenue()
    {
        pauseMenu.SetActive(true);
        //if (MainMenu || pauseMenu || lostMenu || SettingMenu || DontKnowMenu)
        //{
        //    MainMenu.SetActive(false);
        //    pauseMenu.SetActive(true);
        //    lostMenu.SetActive(false);
        //    SettingMenu.SetActive(false);
        //    DontKnowMenu.SetActive(false);
        //}
    }
    public void GameSetting()
    {
        MainMenu.SetActive(false);
        SettingMenu.SetActive(true) ;

    }
    public void Close()
    {

        if (MainMenu || pauseMenu || lostMenu || SettingMenu)
        {
            MainMenu.SetActive(false);
            pauseMenu.SetActive(false);
            lostMenu.SetActive(false);
            SettingMenu.SetActive(false);

            Time.timeScale = 1.0f;
        }
    }
}
