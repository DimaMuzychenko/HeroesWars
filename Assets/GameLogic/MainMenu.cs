using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject settingsP;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }


    public void OpenSettings()
    {
        menu.SetActive(false);
        settingsP.SetActive(true);
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
