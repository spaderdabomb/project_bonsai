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
            bool activeState = !mainMenu.activeSelf;
            mainMenu.SetActive(activeState);
            dimBg.SetActive(activeState);

            if (mainMenu.activeSelf || settingsMenu.activeSelf)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        private void GameSceneKeyPressed()
        {
            foreach (KeyCode vKey in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKey(vKey))
                {
                    //TODO: fix this
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
                toolHolder.GetComponent<ToolHolder>().itemGrid.GetComponent<ItemGrid>().toggleGOs[toolHolderIndex].GetComponent<Toggle>().isOn = true;
            }
            else if (keybindType == SettingsData.KeyBindType.ShowInventory)
            {
                bool activeState = !inventory.activeSelf;
                inventory.SetActive(activeState);
                // dimBg.SetActive(activeState);

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

        public void BeginPlayState()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;
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