using ProjectBonsai.Assets.Scripts.Controllers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static SettingsData;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace ProjectBonsai
{
    public class SettingsController : MonoBehaviour
    {
        public static SettingsController Instance;

        [SerializeField] GameObject keybindMenuMovement, keybindMenuEquipment;
        [SerializeField] GameObject PressAnyKeyPanel;
        [SerializeField] GameObject toolHolder;
        private GameObject currentKeycodeField;
        private GameObject[] keybindMenus;
        private int currentKeycodeColumn;
        private int numKeybindColumns = 2;

        public Dictionary<KeyBindType, Tuple<string, KeyCode, KeyCode, GameObject, GameObject>> keybindData;

        private void Awake()
        {
            if (Instance)
            {
                Destroy(gameObject);
                return;
            }
            DontDestroyOnLoad(gameObject);
            Instance = this;

            // Call these to setup early
            InitSettingsData();
            InitKeybinds();
        }

        void Start()
        {

        }

        private void InitSettingsData()
        {
            // Structure here is KeyBindType, Primary KeyCode, Secondary KeyCode, Primary GameObject, Secondary GameObject
            keybindData = new Dictionary<KeyBindType, Tuple<string, KeyCode, KeyCode, GameObject, GameObject>>()
            {
                { KeyBindType.Forward, new Tuple< string, KeyCode, KeyCode, GameObject, GameObject >("Move Forward", KeyCode.None, KeyCode.None, new GameObject(), new GameObject()) },
                { KeyBindType.Back, new Tuple< string, KeyCode, KeyCode, GameObject, GameObject >("Move Back", KeyCode.None, KeyCode.None, new GameObject(), new GameObject()) },
                { KeyBindType.Left, new Tuple< string, KeyCode, KeyCode, GameObject, GameObject >("Move Left", KeyCode.None, KeyCode.None, new GameObject(), new GameObject()) },
                { KeyBindType.Right, new Tuple < string, KeyCode, KeyCode, GameObject, GameObject > ("Move Right", KeyCode.None, KeyCode.None, new GameObject(), new GameObject()) },
                { KeyBindType.Sprint, new Tuple < string, KeyCode, KeyCode, GameObject, GameObject > ("Sprint", KeyCode.None, KeyCode.None, new GameObject(), new GameObject()) },
                { KeyBindType.Interact, new Tuple < string, KeyCode, KeyCode, GameObject, GameObject > ("Use/Interact", KeyCode.None, KeyCode.None, new GameObject(), new GameObject()) },
                { KeyBindType.Drop, new Tuple < string, KeyCode, KeyCode, GameObject, GameObject > ("Drop", KeyCode.None, KeyCode.None, new GameObject(), new GameObject()) },
                { KeyBindType.Throw, new Tuple < string, KeyCode, KeyCode, GameObject, GameObject > ("Throw", KeyCode.None, KeyCode.None, new GameObject(), new GameObject()) },
                { KeyBindType.ShowInventory, new Tuple < string, KeyCode, KeyCode, GameObject, GameObject > ("Show Inventory", KeyCode.None, KeyCode.None, new GameObject(), new GameObject()) },
                { KeyBindType.Inventory1, new Tuple < string, KeyCode, KeyCode, GameObject, GameObject > ("Inventory 1", KeyCode.None, KeyCode.None, new GameObject(), new GameObject()) },
                { KeyBindType.Inventory2, new Tuple < string, KeyCode, KeyCode, GameObject, GameObject > ("Inventory 2", KeyCode.None, KeyCode.None, new GameObject(), new GameObject()) },
                { KeyBindType.Inventory3, new Tuple < string, KeyCode, KeyCode, GameObject, GameObject > ("Inventory 3", KeyCode.None, KeyCode.None, new GameObject(), new GameObject()) },
                { KeyBindType.Inventory4, new Tuple < string, KeyCode, KeyCode, GameObject, GameObject > ("Inventory 4", KeyCode.None, KeyCode.None, new GameObject(), new GameObject()) },
                { KeyBindType.Inventory5, new Tuple < string, KeyCode, KeyCode, GameObject, GameObject > ("Inventory 5", KeyCode.None, KeyCode.None, new GameObject(), new GameObject()) },
                { KeyBindType.Inventory6, new Tuple < string, KeyCode, KeyCode, GameObject, GameObject > ("Inventory 6", KeyCode.None, KeyCode.None, new GameObject(), new GameObject()) },
                { KeyBindType.Inventory7, new Tuple < string, KeyCode, KeyCode, GameObject, GameObject > ("Inventory 7", KeyCode.None, KeyCode.None, new GameObject(), new GameObject()) },
                { KeyBindType.Inventory8, new Tuple < string, KeyCode, KeyCode, GameObject, GameObject > ("Inventory 8", KeyCode.None, KeyCode.None, new GameObject(), new GameObject()) },
                { KeyBindType.Inventory9, new Tuple < string, KeyCode, KeyCode, GameObject, GameObject > ("Inventory 9", KeyCode.None, KeyCode.None, new GameObject(), new GameObject()) }
            };

            if (keybindData.Count != SettingsData.defaultKeybinds.Count)
            {
                Debug.LogError("Mismatch between keybind data and settings data count: \n" +
                               "SettingsController data and SettingsData 'defaultKeybinds' are different length");
            }



            foreach (var keyValuePair in SettingsData.defaultKeybinds)
            {
                KeyBindType tempKeyBindType = keyValuePair.Key;
                Tuple<string, KeyCode, KeyCode, GameObject, GameObject> tempTuple;
                tempTuple = new Tuple<string, KeyCode, KeyCode, GameObject, GameObject>(keybindData[tempKeyBindType].Item1, SettingsData.defaultKeybinds[keyValuePair.Key].Item1,
                                                                                        SettingsData.defaultKeybinds[keyValuePair.Key].Item2, keybindData[tempKeyBindType].Item4,
                                                                                        keybindData[tempKeyBindType].Item5);
                keybindData[keyValuePair.Key] = tempTuple;
            }
        }

        private void InitKeybinds()
        {
            // Init prefabs
            keybindMenus = new GameObject[2] { keybindMenuMovement, keybindMenuEquipment }; 
            int[] numElements = new int[2] { keybindMenuMovement.GetComponent<SettingsMenuHeader>().numElements,
                                             keybindMenuEquipment.GetComponent<SettingsMenuHeader>().numElements };
            GameObject keybindPrefab_1 = (GameObject)Resources.Load("Prefabs/UI/KeybindRow_1");
            GameObject keybindPrefab_2 = (GameObject)Resources.Load("Prefabs/UI/KeybindRow_2");
            GameObject[] keybindPrefabs = new GameObject[2] { keybindPrefab_2, keybindPrefab_1 };
            int idx = 0;
            int currentMenuIndex = 0;
            int keybindMenuIdx = 0;
            foreach (var defaultKeybind in SettingsData.defaultKeybinds)
            {
                // Instantiate prefab
                KeyBindType currentKeyBindType = defaultKeybind.Key;
                Tuple<string, KeyCode, KeyCode, GameObject, GameObject> currentTuple = keybindData[currentKeyBindType];
                Tuple<string, KeyCode, KeyCode, GameObject, GameObject> newTuple;
                GameObject currentKeybindPrefab = Instantiate(keybindPrefabs[idx % 2], keybindMenus[keybindMenuIdx].transform);
                currentKeybindPrefab.GetComponent<SettingsKeybind>().InitAfterInstantiation(currentTuple.Item1);

                // Assign keybindData gameobjects
                GameObject primaryField = currentKeybindPrefab.GetComponent<SettingsKeybind>().primaryField;
                GameObject secondaryField = currentKeybindPrefab.GetComponent<SettingsKeybind>().secondaryField;
                newTuple = new Tuple<string, KeyCode, KeyCode, GameObject, GameObject>(currentTuple.Item1, currentTuple.Item2, currentTuple.Item3, primaryField, secondaryField);
                keybindData[currentKeyBindType] = newTuple;

                // Switch keybind submenu indexing
                if (numElements[keybindMenuIdx] <= currentMenuIndex + 1)
                {
                    keybindMenuIdx += 1;
                    currentMenuIndex = 0;
                }
                idx++;
                currentMenuIndex++;
            }

            // Set field values in settings menu
            for (int i = 0; i < numKeybindColumns; i++)
            {
                foreach (KeyValuePair<KeyBindType, Tuple<string, KeyCode, KeyCode, GameObject, GameObject>> entry in keybindData)
                {
                    KeyCode[] keybindItems = new KeyCode[2] { entry.Value.Item2, entry.Value.Item3} ;
                    string keyCodeStr = SettingsData.keyNames[keybindItems[i]];
                    TMP_Text[] keyValueText = new TMP_Text[2] { entry.Value.Item4.GetComponent<TextMeshProUGUI>(), entry.Value.Item5.GetComponent<TextMeshProUGUI>() };
                    keyValueText[i].text = keyCodeStr;
                }
            }
        }

        void Update()
        {

        }

        private void OnEnable()
        {
            EventController.OnKeyPress += KeyCodePressed;
        }

        private void OnDisable()
        {
            EventController.OnKeyPress -= KeyCodePressed;
        }

        // Fires when pressing key for new keybind
        private void KeyCodePressed()
        {
            foreach (KeyCode vKey in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKey(vKey) && PressAnyKeyPanel.activeSelf)
                {
                    // Gets the current KeyBindType
                    KeyBindType keybindType;
                    if (currentKeycodeColumn == 0)
                    {
                        keybindType = keybindData.FirstOrDefault(x => x.Value.Item4 == currentKeycodeField).Key;
                        keybindData[keybindType] = new Tuple<string, KeyCode, KeyCode, GameObject, GameObject>(keybindData[keybindType].Item1, vKey, keybindData[keybindType].Item3, 
                                                                                                               keybindData[keybindType].Item4, keybindData[keybindType].Item5);
                    }
                    else
                    {
                        keybindType = keybindData.FirstOrDefault(x => x.Value.Item5 == currentKeycodeField).Key;
                        keybindData[keybindType] = new Tuple<string, KeyCode, KeyCode, GameObject, GameObject>(keybindData[keybindType].Item1, keybindData[keybindType].Item2, vKey,
                                                                                                               keybindData[keybindType].Item4, keybindData[keybindType].Item5);
                    }
                    currentKeycodeField.GetComponent<TextMeshProUGUI>().text = vKey.ToString();
                    PressAnyKeyPanel.SetActive(false);
                }
            }
            UpdateAllKeybindText();
        }

        public void KeyCodeButtonPressed(GameObject keycodeField, int keycodeColumn)
        {
            currentKeycodeField = keycodeField;
            currentKeycodeColumn = keycodeColumn;
        }

        public KeyCode[] GetCurrentKeyCode(SettingsData.KeyBindType keyBindType)
        {
            KeyCode primaryKeyCode = keybindData[keyBindType].Item2;
            KeyCode secondaryKeyCode = keybindData[keyBindType].Item3;
            KeyCode[] returnStuff = new KeyCode[] { primaryKeyCode, secondaryKeyCode };

            return returnStuff;
        }

        public KeyCode GetCurrentKeybind(KeyBindType keybindType)
        {
            KeyCode keyCode = keybindData[keybindType].Item2;
            if (keyCode == KeyCode.None)
            {
                keyCode = keybindData[keybindType].Item3;
            }

            return keyCode;
        }

        public void UpdateAllKeybindText()
        {
            toolHolder.GetComponent<ToolHolder>().itemGrid.GetComponent<ItemGrid>().UpdateAllKeybinds();
        }
    }
}
