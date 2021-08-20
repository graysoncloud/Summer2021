using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TitleSceneManager : MonoBehaviour
{
    public static TitleSceneManager instance;

    public GameObject optionsMenu;
    public GameObject warningMenu;

    public UIButton newGameButton;
    public UIButton resumeGameButton;
    public UIButton optionsButton;
    public UIButton exitGameButton;

    public Slider musicSlider;
    public Slider sfxSlider;

    public SceneChange titleToMR;


    private Color disabledButtonColor = new Color(.4f, .4f, .4f, 1f);

    private void Awake()
    {
        // Singleton Stuff
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

    }

    public void PrepareScene()
    {
        optionsMenu.SetActive(false);
        warningMenu.SetActive(false);

        ToggleButtonInteractability(true);

        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume");

        if (PlayerPrefs.GetInt("ActiveGame") != 1)
        {
            resumeGameButton.GetComponent<EventTrigger>().enabled = false;
            resumeGameButton.GetComponent<TextMeshProUGUI>().color = disabledButtonColor;
        }
        else {
            resumeGameButton.GetComponent<EventTrigger>().enabled = true;
            resumeGameButton.GetComponent<TextMeshProUGUI>().color = resumeGameButton.GetDefaultColor();
        }

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (optionsMenu.gameObject.activeSelf)
                DisableOptionsMenu();

            if (warningMenu.gameObject.activeSelf)
                DisableWarningMenu();

        }
    }

    public void AttemptNewGame()
    {
        if (PlayerPrefs.GetInt("ActiveGame") != 1)
        {
            GameManager.instance.StartNewGame();
            return;
        }

        // If player already has an active game, give them a warning before deleting
        EnableWarningMenu();
    }

    public void EnableWarningMenu()
    {
        warningMenu.SetActive(true);
        ToggleButtonInteractability(false);
    }

    public void DisableWarningMenu()
    {
        warningMenu.SetActive(false);
        ToggleButtonInteractability(true);
    }

    public void EnableOptionsMenu()
    {
        optionsMenu.SetActive(true);
        ToggleButtonInteractability(false);
    }

    public void DisableOptionsMenu()
    {
        optionsMenu.SetActive(false);
        ToggleButtonInteractability(true);
    }

    // Specifically applies to main menu buttons
    public void ToggleButtonInteractability(bool status)
    {
        newGameButton.GetComponent<EventTrigger>().enabled = status;
        if (PlayerPrefs.GetInt("ActiveGame") == 1)
            resumeGameButton.GetComponent<EventTrigger>().enabled = status;
        optionsButton.GetComponent<EventTrigger>().enabled = status;
        exitGameButton.GetComponent<EventTrigger>().enabled = status;
    } 


}
