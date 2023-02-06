using ProjectBonsai.Assets.Scripts.Controllers;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectBonsai.Assets.Scripts.UI
{
    public class CraftingSubmenuUI : MonoBehaviour
    {
        [SerializeField] public TextMeshProUGUI itemTitle;
        [SerializeField] public TextMeshProUGUI itemDescription;
        [SerializeField] public Image itemIcon;
        [SerializeField] public GameObject materialsGrid;
        [SerializeField] public GameObject craftingButton;
        [SerializeField] public ItemData.ItemEnum itemEnum;

        public GameObject[] craftingMaterialSpaces;
        void Start()
        {
            craftingMaterialSpaces = new GameObject[materialsGrid.transform.childCount];
            for (int i = 0; i < materialsGrid.transform.childCount; i++)
            {
                craftingMaterialSpaces[i] = materialsGrid.transform.GetChild(i).gameObject;
            }
        }

        public void CraftButtonPressed()
        {
            GameSceneController.Instance.AddUIItem(itemEnum, 1);
            ItemData.CraftingRecipe craftingRecipe = ItemData.craftingRecipes[itemEnum];
            for (int i = 0; i < craftingRecipe.items.Length; i++)
            {
                if (craftingRecipe.items[i] != ItemData.ItemEnum.Null)
                {
                    GameSceneController.Instance.SubtractUIItem(craftingRecipe.items[i], craftingRecipe.numItems[i]);
                }
            }

            GameObject currentToggle = CraftingMenuController.Instance.GetCurrentSelectedItemToggle();
            CraftingMenuController.Instance.ShowCraftingSubMenu(currentToggle);
        }

        public void SetNewCraftingItem(ItemData.ItemEnum _itemEnum)
        {
           itemTitle.text = ItemData.itemDict[_itemEnum].name;
           itemIcon.sprite = Resources.Load<Sprite>(GlobalData.uiItemsPSDPath + ItemData.itemDict[_itemEnum].uiIconRef);
           itemDescription.text = ItemData.itemDict[_itemEnum].description;
           itemEnum = _itemEnum;
        }

        public void DestroyCraftingSubMenuItems()
        {
            for (int i = 0; i < materialsGrid.transform.childCount; i++)
            {
                GameObject materialSlot = materialsGrid.transform.GetChild(i).gameObject;
                Core.DestroyAllChildren(materialSlot);

            }
        }
    }
}
