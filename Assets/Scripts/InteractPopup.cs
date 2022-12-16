using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


namespace ProjectBonsai
{
    public class InteractPopup : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI keyBindLabel;
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void ShowPopup()
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
                keyBindStr = keyBind[1].ToString();
            }
            else
            {
                keyBindStr = keyBind[0].ToString();
            }
            keyBindLabel.text = keyBind[0].ToString();
        }

        public void HidePopup()
        {
            gameObject.SetActive(false);    
        }
    }
}
