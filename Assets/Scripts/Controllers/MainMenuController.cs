using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectBonsai.Assets.Scripts.Controllers
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] GameObject mainMenu;
        [SerializeField] GameObject settingsMenu;
        [SerializeField] GameObject achievementsMenu;
        [SerializeField] GameObject highscoresMenu;
        [SerializeField] GameObject levelSelectPanel;
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void ChangeMenu(string menuName)
        {
            if (menuName == "Main")
            {
                mainMenu.SetActive(true);
                settingsMenu.SetActive(false);
                achievementsMenu.SetActive(false);
                highscoresMenu.SetActive(false);
                levelSelectPanel.SetActive(false);
            }
            else if (menuName == "Settings")
            {
                mainMenu.SetActive(false);
                settingsMenu.SetActive(true);
                achievementsMenu.SetActive(false);
                highscoresMenu.SetActive(false);
                levelSelectPanel.SetActive(false);
            }
            else if (menuName == "Achievements")
            {
                mainMenu.SetActive(false);
                settingsMenu.SetActive(false);
                achievementsMenu.SetActive(true);
                highscoresMenu.SetActive(false);
                levelSelectPanel.SetActive(false);
            }
            else if (menuName == "Highscores")
            {
                mainMenu.SetActive(false);
                settingsMenu.SetActive(false);
                achievementsMenu.SetActive(false);
                highscoresMenu.SetActive(true);
                levelSelectPanel.SetActive(false);
            }
            else if (menuName == "LevelSelect")
            {
                mainMenu.SetActive(false);
                settingsMenu.SetActive(false);
                achievementsMenu.SetActive(false);
                highscoresMenu.SetActive(false);
                levelSelectPanel.SetActive(true);
            }
        }
    }
}