using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectBonsai
{
    public class CraftingItemUI : MonoBehaviour
    {
        [SerializeField] public GameObject currentItem;
        [SerializeField] public GameObject itemName;

        public ItemData.ItemEnum itemEnumType;
        void Start()
        {
        
        }
    }
}
