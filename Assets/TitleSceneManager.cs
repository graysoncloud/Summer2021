using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TitleSceneManager : MonoBehaviour
{
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
            StartNewGame();
            return;
        }

        // If player already has an active game, give them a warning before deleting
        EnableWarningMenu();
    }

    public void StartNewGame()
    {
        ToggleButtonInteractability(false);

        float musicVol = PlayerPrefs.GetFloat("MusicVolume");
        float sfxVol = PlayerPrefs.GetFloat("SFXVolume");

        PlayerPrefs.DeleteAll();

        // Saves settings from game to game
        PlayerPrefs.SetFloat("MusicVolume", musicVol);
        PlayerPrefs.SetFloat("SFXVolume", sfxVol);

        foreach (CharacterName character in System.Enum.GetValues(typeof(CharacterName)))
        {
            // Load save state here
            PlayerPrefs.SetInt(character.ToString() + "Attitude", 0);
        }

        PlayerPrefs.SetInt("ActiveGame", 1);
        SceneChangeManager.instance.StartSceneChange(titleToMR);
    }

    public void ResumeGame()
    {
        ToggleButtonInteractability(false);
        SceneChangeManager.instance.StartSceneChange(titleToMR);
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
