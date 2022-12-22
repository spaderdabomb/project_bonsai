using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using ProjectBonsai.Assets.Scripts.Controllers;

namespace ProjectBonsai
{
    public class GridSpaceUI : MonoBehaviour, IDropHandler
    {
        [SerializeField] Toggle m_Toggle;
        [SerializeField] public GameObject grid;

        public int gridIndex;

        GameObject itemGridGO;
        ItemGrid itemGrid;

        void Start()
        {
            itemGridGO = Core.GetFirstParentWithTag(this.gameObject, "ItemGrid");
            itemGrid = itemGridGO.GetComponent<ItemGrid>();
            m_Toggle.onValueChanged.AddListener(delegate { ToggleValueChanged(m_Toggle); });
        }

        public void InitGridSpace(int _gridIndex)
        {
            gridIndex = _gridIndex;
        }

        void ToggleValueChanged(Toggle toggleChanged)
        {
            if (toggleChanged.isOn)
            {
                // print("toggle is on " + toggleChanged.ToString());
            }
            else if (!toggleChanged.isOn)
            {
                // print("toggle is off " + toggleChanged.ToString());
            }
        }

        public GameObject GetCurrentItemUI()
        {
            GameObject currentItemUI = Core.FindGameObjectInChildWithTag(grid, "UIItem");
            return currentItemUI;
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (grid.transform.childCount == 0)
            {
                GameObject droppedItemUI = eventData.pointerDrag;
                droppedItemUI.GetComponent<ItemUI>().gridParent = grid;
            }
            else if (grid.transform.childCount == 1)
            {
                GameObject droppedItemUI = eventData.pointerDrag;
                GameObject droppedItemUIParent = droppedItemUI.transform.parent.gameObject;

                GameObject currentItemUI = GetCurrentItemUI();
                currentItemUI.transform.SetParent(droppedItemUIParent.transform);
                droppedItemUI.GetComponent<ItemUI>().gridParent = grid;
            }
            else
            {
                Debug.LogError("More than 1 item on grid");
            }
        }
    }
}
