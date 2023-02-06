using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using ProjectBonsai.Assets.Scripts.Controllers;


namespace ProjectBonsai
{
    public class SettingsKeybind : MonoBehaviour
    {
        public string keyBindName;
        [SerializeField] public GameObject primaryKeybindBtn;
        [SerializeField] public GameObject secondaryKeybindBtn;
        [SerializeField] public GameObject primaryField;
        [SerializeField] public GameObject secondaryField;
        [SerializeField] public TextMeshProUGUI keybindName;

        GameObject pressAnyKeyPanel;

        void Start()
        {
            pressAnyKeyPanel = Core.FindHiddenGameObjectByNameAndTag("PressAnyKeyPanel", "UI");
            primaryKeybindBtn.GetComponent<Button>().onClick.AddListener(delegate { BtnPressed(); });
            primaryKeybindBtn.GetComponent<Button>().onClick.AddListener(delegate { SettingsController.Instance.KeyCodeButtonPressed(primaryField, 0); });
            secondaryKeybindBtn.GetComponent<Button>().onClick.AddListener(delegate { BtnPressed(); });
            secondaryKeybindBtn.GetComponent<Button>().onClick.AddListener(delegate { SettingsController.Instance.KeyCodeButtonPressed(secondaryField, 1); });
        }

        public void InitAfterInstantiation(string keybindNameStr)
        {
            keybindName.text = keybindNameStr;
        }
        void Update()
        {

        }

        void BtnPressed()
        {
            pressAnyKeyPanel.SetActive(true);
        }
    }
}
