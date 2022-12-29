using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static ItemData;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

namespace ProjectBonsai
{
    public class CraftingMenuController : MonoBehaviour
    {
        public static CraftingMenuController Instance;

        [SerializeField] GameObject craftingMenuGrid;
        [SerializeField] GameObject craftingItemGrid;
        [SerializeField] GameObject craftingSubMenu;
        [SerializeField] GameObject craftingButton;

        [SerializeField] public GameObject toolHolder, inventory;

        [HideInInspector] public GameObject[] menuToggleGOs;
        [HideInInspector] public int numCraftingMenus;

        private GameObject itemPrefab_1;

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
            itemPrefab_1 = (GameObject)Resources.Load("Prefabs/UI/CraftingItemUI");
            numCraftingMenus = craftingMenuGrid.transform.childCount;
            menuToggleGOs = new GameObject[numCraftingMenus];
            for (int i = 0; i < numCraftingMenus; i++)
            {
                menuToggleGOs[i] = craftingMenuGrid.transform.GetChild(i).gameObject;
            }

            // Start at first menu
            LoadMenuItems(0);
            //ShowCraftingSubMenu();
        }

        public GameObject GetCurrentSelectedMenuToggle()
        {
            GameObject currentMenuToggle = craftingMenuGrid.GetComponent<ToggleGroup>().GetFirstActiveToggle().gameObject;

            return currentMenuToggle;
        }

        public GameObject GetCurrentSelectedItemToggle()
        {
            GameObject currentMenuToggle = craftingItemGrid.GetComponent<ToggleGroup>().GetFirstActiveToggle().gameObject;

            return currentMenuToggle;
        }

        public void LoadMenuItems(int menuIdx)
        {
            int currentItemIdx = 0;
            foreach (KeyValuePair<ItemData.ItemEnum, ItemData.CraftingRecipe> keyValuePair in ItemData.craftingRecipes)
            {
                if (keyValuePair.Value.menuIndex == menuIdx)
                {
                    ItemData.ItemStruct itemStructRef = ItemData.itemDict[keyValuePair.Key];
                    GameObject newItem = (GameObject)Instantiate(itemPrefab_1, craftingItemGrid.transform);
                    string spriteName = itemStructRef.uiIconRef;
                    Sprite newSprite = Resources.Load<Sprite>(GlobalData.uiItemsPSDPath + spriteName);
                    newItem.GetComponent<CraftingItemUI>().itemEnumType = keyValuePair.Key;
                    newItem.GetComponent<CraftingItemUI>().currentItem.GetComponent<Image>().sprite = newSprite;
                    newItem.GetComponent<CraftingItemUI>().itemName.GetComponent<TextMeshProUGUI>().text = itemStructRef.name;
                    newItem.GetComponent<Toggle>().group = craftingItemGrid.GetComponent<ToggleGroup>();
                    newItem.GetComponent<Toggle>().onValueChanged.AddListener(delegate { ItemToggleChanged(newItem.GetComponent<Toggle>()); });

                }
                currentItemIdx++;
            }
        }

        public void ShowCraftingSubMenu(GameObject selectedToggleGO)
        {
            // Get info about selected crafting menu item selected
            bool itemCraftable = true;
            craftingSubMenu.SetActive(true);
            craftingSubMenu.GetComponent<CraftingSubmenuUI>().DestroyCraftingSubMenuItems();
            CraftingItemUI currentCraftingItemUI = selectedToggleGO.GetComponent<CraftingItemUI>();
            ItemData.ItemEnum itemEnumType = currentCraftingItemUI.itemEnumType;
            ItemData.CraftingRecipe craftingRecipe = ItemData.craftingRecipes[itemEnumType];

            // Set submenu icons and material grid icons/values
            craftingSubMenu.GetComponent<CraftingSubmenuUI>().SetNewCraftingItem(itemEnumType);
            for (int i = 0; i < craftingRecipe.items.Length; i++)
            {
                if (craftingRecipe.items[i] != ItemData.ItemEnum.Null)
                {
                    // Update crafting materials
                    GameObject itemMaterialPrefab = (GameObject)Resources.Load(GlobalData.uiPrefabPath + "ItemMaterialUI");
                    GameObject itemMaterialUI = Instantiate(itemMaterialPrefab, craftingSubMenu.GetComponent<CraftingSubmenuUI>().craftingMaterialSpaces[i].transform);
                    ItemData.ItemEnum currentItemEnum = craftingRecipe.items[i];
                    itemMaterialUI.GetComponent<ItemMaterialUI>().itemName.text = ItemData.itemDict[currentItemEnum].name;
                    itemMaterialUI.GetComponent<ItemMaterialUI>().quantity.text = craftingRecipe.numItems[i].ToString();
                    itemMaterialUI.GetComponent<ItemMaterialUI>().itemImage.sprite = Resources.Load<Sprite>(GlobalData.uiItemsPSDPath + ItemData.itemDict[currentItemEnum].uiIconRef);

                    // Check if item is craftable
                    ItemMaterialUI craftingItemUI = itemMaterialUI.GetComponent<ItemMaterialUI>().GetComponentInChildren<ItemMaterialUI>();
                    int toolHolderItemQuantity = toolHolder.GetComponent<ToolHolder>().itemGrid.GetComponent<ItemGrid>().GetTotalItemQuantity(craftingRecipe.items[i]);
                    int inventoryItemQuantity = inventory.GetComponent<Inventory>().itemGrid.GetComponent<ItemGrid>().GetTotalItemQuantity(craftingRecipe.items[i]);
                    int totalItemQuantity = toolHolderItemQuantity + inventoryItemQuantity;
                    if (craftingRecipe.numItems[i] > totalItemQuantity)
                    {
                        craftingItemUI.itemName.color = GlobalData.uiItemDisabledGray;
                        craftingItemUI.quantity.color = GlobalData.uiItemDisabledGray;
                        craftingItemUI.itemImage.color = GlobalData.uiItemDisabledGray;
                        itemCraftable = false;
                    }
                }
            }

            craftingButton.GetComponent<Button>().interactable = itemCraftable;
        }

        public void DestroyMenuItems()
        {
            foreach (Transform child in craftingItemGrid.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
        }

        public void OnToggleValueChanged()
        {
            DestroyMenuItems();

            // TODO: Fix this
            LoadMenuItems(0);
        }

        public void ItemToggleChanged(Toggle toggleChanged)
        {
            if (toggleChanged.isOn)
            {
                craftingSubMenu.GetComponent<CraftingSubmenuUI>().DestroyCraftingSubMenuItems();
                ShowCraftingSubMenu(toggleChanged.gameObject);
            }
            else if (!toggleChanged.isOn)
            {
                
            }
        }
    }
}
