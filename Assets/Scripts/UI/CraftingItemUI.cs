using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectBonsai.Assets.Scripts.UI
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
