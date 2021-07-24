using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EscapeMenu : MonoBehaviour
{
    public static EscapeMenu instance = null;

    public GameObject mainMenu;
    public GameObject optionsMenu;
    public GameObject titleWarningMenu;
    public GameObject exitWarningMenu;

    public Slider musicSlider;
    public Slider sfxSlider;

    public SceneChange escapeMenuToTitleSC;

    private void Awake()
    {
        // Singleton Stuff
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (optionsMenu.gameObject.activeSelf || titleWarningMenu.gameObject.activeSelf || exitWarningMenu.gameObject.activeSelf)
                OpenMainMenu();

            else if (mainMenu.gameObject.activeSelf)
                ResumeGame();

        }
    }

    public void ResumeGame()
    {
        GameManager.instance.optionsMenuActive = false;
        this.gameObject.SetActive(false);
    }

    public void OpenOptionsMenu()
    {
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume");
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void OpenTitleWarningMenu()
    {
        mainMenu.SetActive(false);
        titleWarningMenu.SetActive(true);
    }

    public void OpenExitWarningMenu()
    {
        mainMenu.SetActive(false);
        exitWarningMenu.SetActive(true);
    }

    public void OpenMainMenu()
    {
        optionsMenu.SetActive(false);
        exitWarningMenu.SetActive(false);
        titleWarningMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void QuitToTitle()
    {
        GameManager.instance.optionsMenuActive = false;
        SceneChangeManager.instance.StartSceneChange(escapeMenuToTitleSC);
        gameObject.SetActive(false);
    }


}
