using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button backFromSettings;
    [SerializeField] private Button backFromCredits;
    [SerializeField] private Button exitButton;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject creditsPanel;
    [SerializeField] private Slider musicVolumeSlider;

    private void Awake()
    {
        settingsMenu.SetActive(false);
        
        playButton.onClick.AddListener(() =>
        {
            PlayerPrefs.DeleteAll();
            SceneManager.LoadScene("Level 1.1");
            Time.timeScale = 1;
        } );
        
        creditsButton.onClick.AddListener(() =>
        {
            creditsPanel.SetActive(true);
        } );
        
        settingsButton.onClick.AddListener(() =>
        {
            settingsMenu.SetActive(true);
        } );
        
        backFromSettings.onClick.AddListener(() =>
        {
            settingsMenu.SetActive(false);
        } );
        
        backFromCredits.onClick.AddListener(() =>
        {
            creditsPanel.SetActive(false);
        } );
        
        exitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        } );
    }

    private void Start()
    {
        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 1);
            Load();
        }
        else
        {
            Load();
        }
    }

    public void ChangeVolume()
    {
        AudioListener.volume = musicVolumeSlider.value;
        Save();
    }

    private void Load()
    {
        musicVolumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
    }

    private void Save()
    {
        PlayerPrefs.SetFloat("musicVolume", musicVolumeSlider.value);
    }
}
