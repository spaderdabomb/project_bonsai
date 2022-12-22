using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectBonsai;
using System.Linq;
using UnityEngine.UI;

using static SettingsData;
using System.Reflection;
using System;
using UnityStandardAssets.Characters.FirstPerson;

namespace ProjectBonsai.Assets.Scripts.Controllers
{
    public class GameSceneController : MonoBehaviour
    {
        public static GameSceneController Instance;
        [SerializeField] Player player;
        [SerializeField] GameObject SettingsMenuController;
        [SerializeField] public GameObject mainMenu, settingsMenu;
        [SerializeField] public GameObject dimBg;
        [SerializeField] public GameObject toolHolder, inventory;
        [SerializeField] public Camera mainCamera;

        int currentSpawnIndex = 0;

        private void Awake()
        {
            if (Instance)
            {
                Destroy(gameObject);
                return;
            }
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        void Start()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            SettingsMenuController.SetActive(true);
            SettingsMenuController.SetActive(false);
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
            }
            else if (menuName == "Settings")
            {
                mainMenu.SetActive(false);
                settingsMenu.SetActive(true);
            }
        }

        public void EscapePressed()
        {
            bool mainMenuActiveState = mainMenu.activeSelf;
            bool settingsMenuActiveState = settingsMenu.activeSelf;

            if (!mainMenuActiveState && !settingsMenuActiveState)
            {
                mainMenu.SetActive(true);
                dimBg.SetActive(true);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                player.GetComponent<FirstPersonController>().enabled = false;
            }
            else if (settingsMenu.activeSelf)
            {
                settingsMenu.SetActive(false);
                mainMenu.SetActive(true);
                dimBg.SetActive(true);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                player.GetComponent<FirstPersonController>().enabled = false;
            }
            else
            {
                ResumePlayState();
            }
        }

        private void GameSceneKeyPressed()
        {
            foreach (KeyCode vKey in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKey(vKey))
                {
                    var keybindType = SettingsController.Instance.keybindData.FirstOrDefault(x => x.Value.Item2 == vKey).Key;
                    HandleKeyPress(keybindType);
                }
            }
        }

        private void HandleKeyPress(SettingsData.KeyBindType keybindType)
        {
            if (keybindType == SettingsData.KeyBindType.Interact)
            {
                player.Interact();
            }
            else if (Core.Contains(SettingsData.inventoryKeyBindTypes, keybindType))
            {
                int toolHolderIndex = Core.GetIndex(SettingsData.inventoryKeyBindTypes, keybindType);
                toolHolder.GetComponent<ToolHolder>().itemGrid.GetComponent<ItemGrid>().gridSpaces[toolHolderIndex].GetComponent<Toggle>().isOn = true;
            }
            else if (keybindType == SettingsData.KeyBindType.ShowInventory)
            {
                bool activeState = !inventory.activeSelf;
                inventory.SetActive(activeState);

                if (inventory.activeSelf)
                {
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                    player.GetComponent<FirstPersonController>().enabled = false;
                }
                else
                {
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                    player.GetComponent<FirstPersonController>().enabled = true;
                }
            }
        }

        public void ResumePlayState()
        {
            mainMenu.SetActive(false);
            settingsMenu.SetActive(false);
            dimBg.SetActive(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            player.GetComponent<FirstPersonController>().enabled = true;
        }

        private void OnEnable()
        {
            EventController.OnKeyPress += GameSceneKeyPressed;
        }

        private void OnDisable()
        {
            EventController.OnKeyPress -= GameSceneKeyPressed;
        }
    }
}