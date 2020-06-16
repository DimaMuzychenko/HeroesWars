using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Ad : MonoBehaviour
{
    [SerializeField] private string url;

    public void OpenUrl()
    {
        Application.OpenURL(url);
    }

    public void GoToGame()
    {
        SceneManager.LoadScene(2);
    }

}
