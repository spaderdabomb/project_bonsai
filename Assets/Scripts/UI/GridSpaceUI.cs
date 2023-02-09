using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using ProjectBonsai.Assets.Scripts.Controllers;
using TMPro;

namespace ProjectBonsai.Assets.Scripts.UI
{
    public class GridSpaceUI : MonoBehaviour, IDropHandler
    {
        [SerializeField] Toggle m_Toggle;
        [SerializeField] public GameObject grid;
        [SerializeField] public TextMeshProUGUI quantityText;
            
        public int itemQuantity { get; private set; }

        public int gridIndex;
        public Vector3 quantityTextOffset;

        GameObject itemGridGO;
        ItemGrid itemGrid;

        void Start()
        {
            itemGridGO = Core.GetFirstParentWithTag(this.gameObject, "ItemGrid");
            itemGrid = itemGridGO.GetComponent<ItemGrid>();
            m_Toggle.onValueChanged.AddListener(delegate { ToggleValueChanged(m_Toggle); });
            itemQuantity = 0;
            quantityText.text = "";
            quantityTextOffset = quantityText.transform.localPosition;
        }

        public void InitGridSpace(int _gridIndex)
        {
            gridIndex = _gridIndex;
        }

        void ToggleValueChanged(Toggle toggleChanged)
        {
            if (toggleChanged.isOn)
            {
                GameObject currentEquippedItem = Core.FindChildGameObjectWithTag(Player.Instance.playerCamera, "ItemEquipped");
                Destroy(currentEquippedItem);
                GameObject itemUI = GetCurrentItemUI();
                if (itemUI != null)
                {
                    GameSceneController.Instance.InstantiateEquippedItem(itemUI.GetComponent<ItemUI>().itemEnum);
                }
            }
            else if (!toggleChanged.isOn)
            {
                // print("toggle is off " + toggleChanged.ToString());
            }
        }

        public GameObject GetCurrentItemUI()
        {
            GameObject currentItemUI = Core.FindChildGameObjectWithTag(grid, "UIItem");
            return currentItemUI;
        }

        public void IncSetItemQuantity(int quantity)
        {
            itemQuantity += quantity;
            if (itemQuantity > 0)
            {
                quantityText.text = itemQuantity.ToString();
            }
            else
            {
                quantityText.text = "";
            }
        }

        public void SetItemQuantity(int quantity)
        {
            itemQuantity = quantity;
            if (itemQuantity > 0)
            {
                quantityText.text = itemQuantity.ToString();
            }
            else
            {
                quantityText.text = "";
            }
        }

        public void OnDrop(PointerEventData eventData)
        {
            // Non-swapping (slot is empty)
            if (grid.transform.childCount == 0)
            {
                GameObject droppedItemUI = eventData.pointerDrag;
                
                // Returns if can't find ItemUI
                if (!droppedItemUI.TryGetComponent<ItemUI>(out var itemUI)) 
                    return;

                // Update itemUI to new gridSpaceUI
                GameObject gridSpaceUI = itemUI.GetGridSpaceUIParent();
                int currentQuantity = gridSpaceUI.GetComponent<GridSpaceUI>().itemQuantity;
                this.SetItemQuantity(currentQuantity);
                gridSpaceUI.GetComponent<GridSpaceUI>().SetItemQuantity(0);
                droppedItemUI.transform.SetParent(grid.transform);
            }
            // Swapping items
            else if (grid.transform.childCount == 1)
            {
                GameObject droppedItemUI = eventData.pointerDrag;
                GameObject droppedGridSpaceUI = droppedItemUI.GetComponent<ItemUI>().GetGridSpaceUIParent();
                int droppedItemUIQuantity = droppedGridSpaceUI.GetComponent<GridSpaceUI>().itemQuantity;

                GameObject currentItemUI = GetCurrentItemUI();
                GameObject currentGridSpaceUI = currentItemUI.GetComponent<ItemUI>().GetGridSpaceUIParent();
                int currentItemUIQuantity = currentGridSpaceUI.GetComponent<GridSpaceUI>().itemQuantity;

                currentItemUI.transform.SetParent(droppedGridSpaceUI.GetComponent<GridSpaceUI>().grid.transform);
                droppedItemUI.transform.SetParent(currentGridSpaceUI.GetComponent<GridSpaceUI>().grid.transform);
                droppedGridSpaceUI.GetComponent<GridSpaceUI>().SetItemQuantity(currentItemUIQuantity);
                currentGridSpaceUI.GetComponent<GridSpaceUI>().SetItemQuantity(droppedItemUIQuantity);
                droppedGridSpaceUI.GetComponent<GridSpaceUI>().quantityText.transform.localPosition = currentGridSpaceUI.GetComponent<GridSpaceUI>().quantityTextOffset;
                currentGridSpaceUI.GetComponent<GridSpaceUI>().quantityText.transform.localPosition = droppedGridSpaceUI.GetComponent<GridSpaceUI>().quantityTextOffset;
            }
            else
            {
                Debug.LogError("More than 1 item on grid");
            }
        }
    }
}
