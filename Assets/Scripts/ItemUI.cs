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

        public GameObject gridParent;
        public Vector2 startPosition;

        private GameObject itemGridGO;
        ItemGrid itemGrid;
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
            gridParent = transform.parent.gameObject;
            itemStruct = ItemData.itemDict[itemEnum];
            itemGridGO = Core.GetFirstParentWithTag(this.gameObject, "ItemGrid");
            itemGrid = itemGridGO.GetComponent<ItemGrid>();
        }

        // Update is called once per frame
        void Update()
        {
        
        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            startPosition = transform.position;
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = Input.mousePosition;
            this.gameObject.GetComponent<Image>().raycastTarget = false;

        }

        public void OnEndDrag(PointerEventData eventData)
        {
            this.gameObject.GetComponent<Image>().raycastTarget = true;
            this.transform.SetParent(gridParent.transform);
            transform.position = startPosition;
        }
    }
}
