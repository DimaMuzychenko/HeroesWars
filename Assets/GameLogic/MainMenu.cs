using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using Assets.GameLogic;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject settingsP;
    [SerializeField] private Assets.GameLogic.UnitClasses.PrefabsList prefabsList;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundsSlider;

    public static float soundsVolume;

    private int sceneCount;

    private void Awake()
    {
        soundsVolume = 1f;
        sceneCount = 1;
    }



    public void OpenSettings()
    {
        menu.SetActive(false);
        settingsP.SetActive(true);
    }
    public void ChangeMusicVolume()
    {
        Camera.main.GetComponent<AudioSource>().volume = musicSlider.value;
    }

    public void ChangeSoundsVolume()
    {
        soundsVolume = soundsSlider.value;
    }

    public void CloseSettings()
    {
        settingsP.SetActive(false);
        menu.SetActive(true);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(1);       
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
