using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using ProjectBonsai.Assets.Scripts.Controllers;

public class ButtonOverride : MonoBehaviour
{
    MainMenuController mainMenuController;
    GameObject buttonGO;
    Button button;

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            mainMenuController = Core.FindGameObjectByNameAndTag("MainMenuController", "Menu Item").GetComponent<MainMenuController>();
        }

        buttonGO = gameObject;
        button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(delegate { ButtonClicked(buttonGO); });
    }

    public void ButtonClicked(GameObject buttonGO)
    {
        // Main menu buttons
/*        if (buttonGO.name == "SettingsButton")
        {
            mainMenuController.ChangeMenu("Settings");
        }
        else if (buttonGO.name == "AchievementsButton")
        {
            mainMenuController.ChangeMenu("Achievements");
        }*/
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}