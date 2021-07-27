using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TitleSceneManager : MonoBehaviour
{
    public GameObject optionsMenu;
    public GameObject warningMenu;

    public Button newGameButton;
    public Button resumeGameButton;
    public Button optionsButton;
    public Button exitGameButton;

    public Slider volumeSlider;
    public Slider sfxSlider;

    public SceneChange titleToMR;


    private Color disabledButtonColor = new Color(.7f, .7f, .7f, 1f);
    private Color normalButtonColor = new Color(1f, 1f, 1f, 1f);

    public void PrepareScene()
    {
        optionsMenu.SetActive(false);
        warningMenu.SetActive(false);

        ToggleButtonInteractability(true);

        volumeSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume");

        if (PlayerPrefs.GetInt("ActiveGame") != 1)
        {
            resumeGameButton.interactable = false;
            resumeGameButton.GetComponent<Image>().color = disabledButtonColor;
        }
        else {
            resumeGameButton.interactable = true;
            resumeGameButton.GetComponent<Image>().color = normalButtonColor;
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
        newGameButton.interactable = status;
        resumeGameButton.interactable = status;
        optionsButton.interactable = status;
        exitGameButton.interactable = status;
    } 


}
