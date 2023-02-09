using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace ProjectBonsai
{
    public class PlayerInfoUI : MonoBehaviour
    {
        [SerializeField] private float initialBarWidth;

        private VisualElement root;
        private VisualElement healthBar;
        private VisualElement manaBar;
        private Label ammoRemainingClip;
        private Label ammoRemainingTotal;

        private float currentHealthBarWidth;
        private float currentManaBarWidth;

        private float _healthbarWidthRatio;
        private float HealthbarWidthRatio 
        { 
            get { return _healthbarWidthRatio; } 
            set
            {
                if (value > 1.0f)
                    value = 1.0f;
                if (value < 0.0f)
                    value = 0.0f;
                _healthbarWidthRatio = value;
            }
        }
        private float _manabarWidthRatio;
        private float ManaBarWidthRatio
        {
            get { return _manabarWidthRatio; }
            set
            {
                if (value > 1.0f)
                    value = 1.0f;
                if (value < 0.0f)
                    value = 0.0f;
                _manabarWidthRatio = value;
            }
        }

        // OnEnable necessary in case all UI elements are deleted, we need new references
        void OnEnable()
        {
            root = GetComponent<UIDocument>().rootVisualElement;
            root.RegisterCallback<GeometryChangedEvent>(GeometryChangedCallback);

            healthBar = root.Q<VisualElement>("health-bar");
            manaBar = root.Q<VisualElement>("mana-bar");
            ammoRemainingClip = root.Q<Label>("ammo-remaining-clip");
            ammoRemainingTotal = root.Q<Label>("ammo-remaining-total");
        }

        // Methods for after objects have loaded and have definite geometry
        private void GeometryChangedCallback(GeometryChangedEvent evt)
        {
            root.UnregisterCallback<GeometryChangedEvent>(GeometryChangedCallback);
        }

        // Update runs before GeometryChangedCallback
        private void Update()
        {
            HealthbarWidthRatio = (Player.Instance.playerData.HealthCurrent / Player.Instance.playerData.HealthMax);
            ManaBarWidthRatio = (Player.Instance.playerData.ManaCurrent / Player.Instance.playerData.ManaMax);

            currentHealthBarWidth = _healthbarWidthRatio * initialBarWidth;
            currentManaBarWidth = _manabarWidthRatio * initialBarWidth;
            healthBar.style.width = currentHealthBarWidth;
            manaBar.style.width = currentManaBarWidth;
        }

        public void UpdateAmmoUI()
        {
            // ammo UI stuff
        }
    }
}
