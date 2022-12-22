using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectBonsai
{
    public class CraftingMenuController : MonoBehaviour
    {
        [SerializeField] GameObject craftingMenuGrid;
        [SerializeField] GameObject craftingItemGrid;

        [HideInInspector] public GameObject[] menuToggleGOs;
        [HideInInspector] public int numCraftingMenus;
        [HideInInspector] public List<ItemData.ItemStruct> craftableItems;

        private GameObject itemPrefab_1;
        private GameObject itemPrefab_2;

        void Start()
        {
            itemPrefab_1 = (GameObject)Resources.Load("Prefabs/UI/CraftingItemUI");
            numCraftingMenus = craftingMenuGrid.transform.childCount;
            menuToggleGOs = new GameObject[numCraftingMenus];
            for (int i = 0; i < numCraftingMenus; i++)
            {
                menuToggleGOs[i] = craftingMenuGrid.transform.GetChild(i).gameObject;
            }

            // Get list of craftable items
            craftableItems = new List<ItemData.ItemStruct>();
            foreach (KeyValuePair<ItemData.ItemEnum, ItemData.ItemStruct> keyValuePair in ItemData.itemDict)
            {
                ItemData.ItemStruct itemStruct = keyValuePair.Value;
                if (itemStruct.craftable)
                {
                    craftableItems.Add(itemStruct);
                }
            }

            // Start at first menu
            LoadMenuItems(0);
        }

        public void LoadMenuItems(int menuIdx)
        {
            int currentItemIdx = 0;
            foreach (ItemData.ItemStruct itemStruct in craftableItems)
            {
                if (itemStruct.craftingMenuIdx == menuIdx)
                {
                    GameObject newItem = (GameObject)Instantiate(itemPrefab_1, craftingItemGrid.transform);
                    string spriteName = itemStruct.uiIconRef;
                    Sprite newSprite = Resources.Load<Sprite>(GlobalData.uiItemsPSDPath + spriteName);
                    newItem.GetComponent<CraftingItemUI>().currentItem.GetComponent<Image>().sprite = newSprite;
                    newItem.GetComponent<CraftingItemUI>().itemName.GetComponent<TextMeshProUGUI>().text = itemStruct.name;
                    newItem.GetComponent<Toggle>().group = craftingItemGrid.GetComponent<ToggleGroup>();

                }
                currentItemIdx++;
            }
        }

        public void DestroyCurrentMenuItems()
        {
            foreach (Transform child in craftingItemGrid.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
        }

        public void OnToggleValueChanged()
        {
            DestroyCurrentMenuItems();

            // TODO: Fix this
            LoadMenuItems(0);
        }
    }
}
