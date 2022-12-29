using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectBonsai
{
    public class ItemMaterialUI : MonoBehaviour
    {
        [SerializeField] public TextMeshProUGUI itemName;
        [SerializeField] public TextMeshProUGUI quantity;
        [SerializeField] public Image itemImage;
    }
}
