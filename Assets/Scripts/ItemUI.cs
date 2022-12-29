using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace ProjectBonsai
{
    public class ItemUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] public ItemData.ItemEnum itemEnum;
        [SerializeField] public ItemData.ItemStruct itemStruct;

        [HideInInspector] public Vector2 startPosition;

        Canvas itemUICanvas;

        private void Awake()
        {
            itemUICanvas = this.gameObject.GetComponent<Canvas>();
            itemUICanvas.overrideSorting = true;
            itemUICanvas.sortingOrder = 1;
        }
        void Start()
        {
            startPosition = transform.position;
            itemStruct = ItemData.itemDict[itemEnum];
        }

        // Update is called once per frame
        void Update()
        {
        
        }
        public GameObject GetGridSpaceUIParent()
        {
            return Core.GetFirstParentWithTag(this.gameObject, "GridSpace");
        }

        public GameObject GetItemGridParent()
        {
            return Core.GetFirstParentWithTag(this.gameObject, "GridSpace");
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            startPosition = transform.position;
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = Input.mousePosition;
            GameObject gridSpaceParent = GetGridSpaceUIParent();
            gridSpaceParent.GetComponent<GridSpaceUI>().quantityText.transform.position = Input.mousePosition + gridSpaceParent.GetComponent<GridSpaceUI>().quantityTextOffset;
            this.gameObject.GetComponent<Image>().raycastTarget = false;

        }

        public void OnEndDrag(PointerEventData eventData)
        {
            this.gameObject.GetComponent<Image>().raycastTarget = true;
            transform.position = startPosition;
            GameObject gridSpaceParent = GetGridSpaceUIParent();
            gridSpaceParent.GetComponent<GridSpaceUI>().quantityText.transform.localPosition = gridSpaceParent.GetComponent<GridSpaceUI>().quantityTextOffset;
        }
    }
}
