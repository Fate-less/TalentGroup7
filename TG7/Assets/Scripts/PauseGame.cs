using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class PauseGame : MonoBehaviour
{
    [SerializeField] GameObject pausePopup;
    [SerializeField] GameObject pauseSettings;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] TMP_InputField inputField;

    private bool onPause = false;
    private bool onMainmenu = false;

    private void Start()
    {
        try
        {
            inputField = GameObject.Find("InputField").GetComponent<TMP_InputField>();
        }
        catch(NullReferenceException e) { };
        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            onMainmenu = true;
        }
        else
        {
            onMainmenu = false;
        }
    }

    void Update()
    {
        if (onPause)
        {
            Time.timeScale = 0;
            AudioListener.pause = true;
            if(inputField != null) inputField.DeactivateInputField();
        }
        else
        {
            Time.timeScale = 1;
            AudioListener.pause = false;
            if (inputField != null) inputField.ActivateInputField();
        }
        if (!pausePopup.activeInHierarchy && Input.GetKeyDown(KeyCode.Escape) && !onMainmenu)
        {
            onPause = true;
            pausePopup.SetActive(true);
        }
        else if(pausePopup.activeInHierarchy && Input.GetKeyDown(KeyCode.Escape) && pauseSettings.activeInHierarchy && !onMainmenu)
        {
            CloseSettings();
        }
        else if(pausePopup.activeInHierarchy && Input.GetKeyDown(KeyCode.Escape) && !pauseSettings.activeInHierarchy && !onMainmenu)
        {
            Resume();
        }
    }

    public void OpenSettings()
    {
        pauseMenu.SetActive(false);
        pauseSettings.SetActive(true);
    }

    public void CloseSettings()
    {
        pauseMenu.SetActive(true);
        pauseSettings.SetActive(false);
    }

    public void Resume()
    {
        onPause = false;
        pausePopup.SetActive(false);
    }

    public void menuExit()
    {
        SceneManager.LoadScene(0);
    }
}
