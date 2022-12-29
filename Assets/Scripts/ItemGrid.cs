using ProjectBonsai.Assets.Scripts.Controllers;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectBonsai
{
    public class ItemGrid : MonoBehaviour
    {
        [SerializeField] int _rows, _columns;
        [SerializeField] bool alreadyInstantiated;

        public GameObject[] gridSpaces;
        public GameObject[] keybindTexts;

        public void InitItemGrid(int rows, int columns)
        {
            _rows = rows;
            _columns = columns;
        }
        void Start()
        {
            // Deal with already being instantiated vs. loading prefab
            if (alreadyInstantiated)
            {

            }
            else
            {
                alreadyInstantiated = true;
            }

            // Create arrays to reference item grid spaces
            keybindTexts = new GameObject[_rows * _columns];
            gridSpaces = new GameObject[_rows*_columns];
            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _columns; j++)
                {
                    GameObject prefab = (GameObject)Resources.Load("Prefabs/UI/GridSpaceUI");
                    GameObject gridSpace = Instantiate(prefab, this.transform);
                    gridSpace.GetComponent<GridSpaceUI>().InitGridSpace(j + i * _columns);
                    gridSpace.GetComponent<Toggle>().group = this.gameObject.GetComponent<ToggleGroup>();
                    gridSpaces[j + i * _columns] = gridSpace;
                    keybindTexts[j + i * _columns] = gridSpace.transform.Find("Keybind").gameObject;
                }
            }

            gridSpaces[0].GetComponent<Toggle>().isOn = true;


            // Initialize item grid type specific values
            if (gameObject.transform.parent.gameObject.name == "ToolHolder")
            {
                UpdateAllKeybinds();
            }
        }

        public int InstantiateItem(ItemData.ItemEnum itemEnum, int slotIndex, int itemQuantity)
        {
            // Handles if no item in inventory/toolholder
            int currentItemQuantity = 0;
            int currentNumItem = (int)gridSpaces[slotIndex].GetComponent<GridSpaceUI>().itemQuantity;
            if (currentNumItem == 0)
            {
                GameObject item = (GameObject)Resources.Load(GlobalData.uiItemsPrefabPath + ItemData.itemDict[itemEnum].uiIconRef);
                GameObject instantiatedItem = Instantiate(item, gridSpaces[slotIndex].GetComponent<GridSpaceUI>().grid.transform);
                gridSpaces[slotIndex].GetComponent<GridSpaceUI>().IncSetItemQuantity(itemQuantity);
            }
            // Handles if item in inventory/toolholder but new quantity is less than max stack
            else if (currentNumItem + itemQuantity <= ItemData.itemDict[itemEnum].maxStack)
            {
                gridSpaces[slotIndex].GetComponent<GridSpaceUI>().IncSetItemQuantity(itemQuantity);
            }
            // Handles if item in inventory/toolholder but new quantity is more than max stack
            else if (currentNumItem + itemQuantity > ItemData.itemDict[itemEnum].maxStack)
            {
                int loopIndex = 0;
                currentItemQuantity = itemQuantity;
                while (currentItemQuantity > 0) 
                {
                    bool doesNonMaxStackExist = DoesNonMaxItemStackExist(itemEnum);
                    if (doesNonMaxStackExist)
                    {
                        slotIndex = GetFirstNonMaxStackSlotWithItem(itemEnum);
                        currentNumItem = (int)gridSpaces[slotIndex].GetComponent<GridSpaceUI>().itemQuantity;
                        int diffToMax = ItemData.itemDict[itemEnum].maxStack - currentNumItem;
                        gridSpaces[slotIndex].GetComponent<GridSpaceUI>().IncSetItemQuantity(diffToMax);
                        currentItemQuantity -= diffToMax;
                    }
                    else
                    {
                        slotIndex = GetFirstFreeSlotIndex();
                        if (slotIndex != -1)
                        {
                            GameObject item = (GameObject)Resources.Load(GlobalData.uiItemsPrefabPath + ItemData.itemDict[itemEnum].uiIconRef);
                            GameObject instantiatedItem = Instantiate(item, gridSpaces[slotIndex].GetComponent<GridSpaceUI>().grid.transform);
                            int newStackQuantity;
                            if (currentItemQuantity > ItemData.itemDict[itemEnum].maxStack)
                            {
                                newStackQuantity = ItemData.itemDict[itemEnum].maxStack;
                            }
                            else
                            {
                                newStackQuantity = currentItemQuantity;
                            }
                            gridSpaces[slotIndex].GetComponent<GridSpaceUI>().IncSetItemQuantity(newStackQuantity);
                            currentItemQuantity -= newStackQuantity;
                        }
                        else
                        {
                            return currentItemQuantity;
                        }
                    }
                    loopIndex++;
                    if (loopIndex > 100)
                    {
                        Debug.LogError("Something is fucked in ItemGrid>Instantiate Item");
                        break;
                    }
                }
            }

            return currentItemQuantity;
        }

        public void SubtractItemFromGrid()
        {

        }

        public int GetFirstFreeSlotIndex()
        {
            int index = 0;
            foreach (GameObject gridSpace in gridSpaces)
            {
                if (gridSpace.GetComponent<GridSpaceUI>().grid.transform.childCount == 0)
                {
                    return index;
                }
                index++;
            }

            return -1;
        }

        /// <summary>
        /// Returns first slot with passed item, otherwise returns first free slot
        /// </summary>
        /// <param name="itemEnum"></param>
        /// <returns></returns>
        public int GetFirstNonMaxStackSlotWithItem(ItemData.ItemEnum itemEnum)
        {
            int index = 0;
            foreach (GameObject gridSpace in gridSpaces)
            {
                GameObject gridSpaceItem = gridSpace.GetComponent<GridSpaceUI>().GetCurrentItemUI();
                if (gridSpaceItem != null)
                {
                    int currentQuantity = gridSpace.GetComponent<GridSpaceUI>().itemQuantity;
                    if (gridSpaceItem.GetComponent<ItemUI>().itemEnum == itemEnum && currentQuantity != ItemData.itemDict[itemEnum].maxStack)
                    {
                        return index;
                    }
                }
                index++;
            }

            index = GetFirstFreeSlotIndex();

            return index;
        }

        /// <summary>
        /// Returns first slot with passed item, including max stacks, otherwise returns -1
        /// </summary>
        /// <param name="itemEnum"></param>
        /// <returns></returns>
        public int GetFirstSlotWithItem(ItemData.ItemEnum itemEnum)
        {
            int index = 0;
            foreach (GameObject gridSpace in gridSpaces)
            {
                GameObject gridSpaceItem = gridSpace.GetComponent<GridSpaceUI>().GetCurrentItemUI();
                if (gridSpaceItem != null) 
                {
                    if (gridSpaceItem.GetComponent<ItemUI>().itemEnum == itemEnum)
                    {
                        return index;
                    }
                }
                index++;
            }

            return -1;
        }

        public bool DoesNonMaxItemStackExist(ItemData.ItemEnum itemEnum)
        {
            int index = 0;
            foreach (GameObject gridSpace in gridSpaces)
            {
                GameObject gridSpaceItem = gridSpace.GetComponent<GridSpaceUI>().GetCurrentItemUI();
                if (gridSpaceItem != null)
                {
                    int currentQuantity = gridSpace.GetComponent<GridSpaceUI>().itemQuantity;
                    if (gridSpaceItem.GetComponent<ItemUI>().itemEnum == itemEnum && currentQuantity != ItemData.itemDict[itemEnum].maxStack)
                    {
                        return true;
                    }
                }
                index++;
            }

            return false;
        }

        public int GetTotalItemQuantity(ItemData.ItemEnum itemEnum)
        {
            int itemQuantity = 0;
            int index = 0;
            foreach (GameObject gridSpace in gridSpaces)
            {
                GameObject gridSpaceItem = gridSpace.GetComponent<GridSpaceUI>().GetCurrentItemUI();
                if (gridSpaceItem != null)
                {
                    if (gridSpaceItem.GetComponent<ItemUI>().itemEnum == itemEnum)
                    {
                        GameObject gridSpaceUI = gridSpaceItem.GetComponent<ItemUI>().GetGridSpaceUIParent();
                        itemQuantity += gridSpaceUI.GetComponent<GridSpaceUI>().itemQuantity;
                    }
                }
                index++;
            }

            return itemQuantity;
        }

        public void UpdateAllKeybinds()
        {
            int i = 0;
            foreach (SettingsData.KeyBindType keybindType in SettingsData.inventoryKeyBindTypes)
            {
                KeyCode keyCode = SettingsController.Instance.GetCurrentKeybind(keybindType);
                string keyCodeStr = SettingsData.keyNames[keyCode];
                keybindTexts[i].GetComponent<TextMeshProUGUI>().text = keyCodeStr;
                i++;
            }
        }
    }
}
