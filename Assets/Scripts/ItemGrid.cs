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

        public void InstantiateItem(string itemName, int slotIndex)
        {
            GameObject item = (GameObject)Resources.Load("Prefabs/UI/" + itemName);
            GameObject instantiatedItem = Instantiate(item, gridSpaces[slotIndex].GetComponent<GridSpaceUI>().grid.transform);
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

            return index;
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
