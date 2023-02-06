using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using ProjectBonsai.Assets.Scripts.Controllers;


namespace ProjectBonsai.Assets.Scripts.UI
{
    public class InteractPopup : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI keyBindLabel;
        [SerializeField] TextMeshProUGUI interactObjectText;
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
        
        }

        /// <summary>
        /// Shows keybind popup for interacting
        /// </summary>
        /// <param name="itemGameObj"> 3D item model game object </param>
        public void ShowPopup(GameObject itemGameObj)
        {
            this.gameObject.SetActive(true);
            KeyCode[] keyBind = SettingsController.Instance.GetCurrentKeyCode(SettingsData.KeyBindType.Interact);
            string keyBindStr;
            if (keyBind[0] == KeyCode.None && keyBind[0] == KeyCode.None)
            {
                keyBindStr = "-";
            }
            else if (keyBind[0] == KeyCode.None)
            {
                keyBindStr = SettingsData.keyNames[keyBind[1]].ToString();
            }
            else
            {
                keyBindStr = SettingsData.keyNames[keyBind[0]].ToString();
            }
            keyBindLabel.text = "[" + "<color=" + GlobalData.uiOrangeStandardHex + ">"+keyBindStr + "</color>" + "] Pick up";
            string itemStr = ItemData.itemDict[itemGameObj.GetComponent<Item>().itemEnum].name + " x" + itemGameObj.GetComponent<Item>().itemQuantity.ToString();
            interactObjectText.text = itemStr;
        }

        public void HidePopup()
        {
            gameObject.SetActive(false);    
        }
    }
}
