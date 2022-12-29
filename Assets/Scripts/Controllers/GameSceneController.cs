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
using static ItemData;

namespace ProjectBonsai.Assets.Scripts.Controllers
{
    public class GameSceneController : MonoBehaviour
    {
        public static GameSceneController Instance;
        [SerializeField] Player player;
        [SerializeField] GameObject SettingsMenuController;
        [SerializeField] public GameObject mainMenu, settingsMenu;
        [SerializeField] public GameObject dimBg;
        [SerializeField] public GameObject toolHolder, inventory, craftingMenu, playerMenu;
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
                bool activeState = !inventory.GetComponent<Canvas>().enabled;
                inventory.GetComponent<Canvas>().enabled = activeState;
                craftingMenu.GetComponent<Canvas>().enabled = activeState;
                playerMenu.GetComponent<Canvas>().enabled = activeState;

                if (inventory.GetComponent<Canvas>().enabled)
                {
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                    player.GetComponent<FirstPersonController>().enabled = false;
                    CraftingMenuController.Instance.ShowCraftingSubMenu(CraftingMenuController.Instance.GetCurrentSelectedItemToggle());
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

        /// <summary>
        /// Assumes a 3D model will appear on the ground if there is no space
        /// </summary>
        /// <param name="itemEnum"></param>
        /// <param name="itemQuantity"></param>
        /// <returns></returns>
        public int AddUIItem(ItemData.ItemEnum itemEnum, int itemQuantity)
        {
            (GameObject itemGrid, int firstAvailableIndex) = GetIndexToAddItem(itemEnum);
            int leftoverItemQuantity = itemQuantity;
            if (firstAvailableIndex != -1)
            {
                leftoverItemQuantity = itemGrid.GetComponent<ItemGrid>().InstantiateItem(itemEnum, firstAvailableIndex, itemQuantity);
            }

            // If we overflowed from first itemgrid to second itemgrid
            if (leftoverItemQuantity != 0)
            {
                (itemGrid, firstAvailableIndex) = GetIndexToAddItem(itemEnum);
                if (firstAvailableIndex != -1)
                {
                    leftoverItemQuantity = itemGrid.GetComponent<ItemGrid>().InstantiateItem(itemEnum, firstAvailableIndex, leftoverItemQuantity);
                }
            }

            return leftoverItemQuantity;
        }

        /// <summary>
        /// Assumes at least one item exists in inventory/toolholder
        /// </summary>
        /// <param name="itemEnum"></param>
        /// <param name="itemQuantity"></param>
        public void SubtractUIItem(ItemData.ItemEnum itemEnum, int itemQuantity)
        {
            (GameObject itemGrid, int firstAvailableIndex) = GetIndexToSubtractItem(itemEnum);
            int gridItemQuantity = itemGrid.GetComponent<ItemGrid>().gridSpaces[firstAvailableIndex].GetComponent<GridSpaceUI>().itemQuantity;

            // Doesn't destroy itemUI
            if (gridItemQuantity > itemQuantity)
            {
                itemGrid.GetComponent<ItemGrid>().gridSpaces[firstAvailableIndex].GetComponent<GridSpaceUI>().SetItemQuantity(gridItemQuantity - itemQuantity);
            }
            // Destroys itemUIs in loop
            else
            {
                int loopIndex = 0;
                int currentItemsRemaining = itemQuantity;
                while (currentItemsRemaining > 0)
                {
                    currentItemsRemaining -= gridItemQuantity;
                    if (currentItemsRemaining >= 0)
                    {
                        GameObject currentItemUI = itemGrid.GetComponent<ItemGrid>().gridSpaces[firstAvailableIndex].GetComponent<GridSpaceUI>().GetCurrentItemUI();
                        itemGrid.GetComponent<ItemGrid>().gridSpaces[firstAvailableIndex].GetComponent<GridSpaceUI>().SetItemQuantity(0);
                        Destroy(currentItemUI);
                    }
                    else
                    {
                        itemGrid.GetComponent<ItemGrid>().gridSpaces[firstAvailableIndex].GetComponent<GridSpaceUI>().SetItemQuantity(gridItemQuantity - itemQuantity);
                    }
                    (itemGrid, firstAvailableIndex) = GetIndexToSubtractItem(itemEnum);

                    loopIndex++;
                    if (loopIndex > 100)
                    {
                        Debug.LogError("Something is fucked in ItemGrid>Instantiate Item");
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Searches toolholder than inventory for available slot to add item
        /// Returns -1 if no free slot in either
        /// </summary>
        /// <param name="itemEnum"></param>
        /// <returns></returns>
        public (GameObject itemGrid, int itemIndex) GetIndexToAddItem(ItemData.ItemEnum itemEnum)
        {
            int indexToAdd;
            GameObject itemGrid;
            bool itemExistsInToolholder = GameSceneController.Instance.toolHolder.GetComponent<ToolHolder>().itemGrid.GetComponent<ItemGrid>().DoesNonMaxItemStackExist(itemEnum);
            bool itemExistsInInventory = GameSceneController.Instance.inventory.GetComponent<Inventory>().itemGrid.GetComponent<ItemGrid>().DoesNonMaxItemStackExist(itemEnum);
            if (itemExistsInToolholder)
            {
                indexToAdd = GameSceneController.Instance.toolHolder.GetComponent<ToolHolder>().itemGrid.GetComponent<ItemGrid>().GetFirstNonMaxStackSlotWithItem(itemEnum);
                itemGrid = GameSceneController.Instance.toolHolder.GetComponent<ToolHolder>().itemGrid;
            }
            else if (itemExistsInInventory)
            {
                indexToAdd = GameSceneController.Instance.inventory.GetComponent<Inventory>().itemGrid.GetComponent<ItemGrid>().GetFirstNonMaxStackSlotWithItem(itemEnum);
                itemGrid = GameSceneController.Instance.inventory.GetComponent<Inventory>().itemGrid;
            }
            else
            {
                indexToAdd = GameSceneController.Instance.toolHolder.GetComponent<ToolHolder>().itemGrid.GetComponent<ItemGrid>().GetFirstFreeSlotIndex();
                itemGrid = GameSceneController.Instance.toolHolder.GetComponent<ToolHolder>().itemGrid;
                if (indexToAdd == -1)
                {
                    indexToAdd = GameSceneController.Instance.inventory.GetComponent<Inventory>().itemGrid.GetComponent<ItemGrid>().GetFirstFreeSlotIndex();
                    itemGrid = GameSceneController.Instance.inventory.GetComponent<Inventory>().itemGrid;
                }
            }

            return (itemGrid, indexToAdd);
        }

        /// <summary>
        /// Searches toolholder than inventory for available slot to add item
        /// Returns -1 if there are no items available to subtract
        /// </summary>
        /// <param name="itemEnum"></param>
        /// <returns></returns>
        public (GameObject itemGrid, int itemIndex) GetIndexToSubtractItem(ItemData.ItemEnum itemEnum)
        {
            int indexToSubtract;
            GameObject itemGrid;
            bool itemExistsInToolholder = GameSceneController.Instance.toolHolder.GetComponent<ToolHolder>().itemGrid.GetComponent<ItemGrid>().DoesNonMaxItemStackExist(itemEnum);
            bool itemExistsInInventory = GameSceneController.Instance.inventory.GetComponent<Inventory>().itemGrid.GetComponent<ItemGrid>().DoesNonMaxItemStackExist(itemEnum);

            if (itemExistsInToolholder)
            {
                indexToSubtract = GameSceneController.Instance.toolHolder.GetComponent<ToolHolder>().itemGrid.GetComponent<ItemGrid>().GetFirstNonMaxStackSlotWithItem(itemEnum);
                itemGrid = GameSceneController.Instance.toolHolder.GetComponent<ToolHolder>().itemGrid;
            }
            else if (itemExistsInInventory)
            {
                indexToSubtract = GameSceneController.Instance.inventory.GetComponent<Inventory>().itemGrid.GetComponent<ItemGrid>().GetFirstNonMaxStackSlotWithItem(itemEnum);
                itemGrid = GameSceneController.Instance.inventory.GetComponent<Inventory>().itemGrid;
            }
            else
            {
                indexToSubtract = GameSceneController.Instance.toolHolder.GetComponent<ToolHolder>().itemGrid.GetComponent<ItemGrid>().GetFirstSlotWithItem(itemEnum);
                itemGrid = GameSceneController.Instance.toolHolder.GetComponent<ToolHolder>().itemGrid;
                if (indexToSubtract == -1)
                {
                    indexToSubtract = GameSceneController.Instance.inventory.GetComponent<Inventory>().itemGrid.GetComponent<ItemGrid>().GetFirstSlotWithItem(itemEnum);
                    itemGrid = GameSceneController.Instance.inventory.GetComponent<Inventory>().itemGrid;
                }
            }

            return (itemGrid, indexToSubtract);
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