using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static ItemData;

namespace ProjectBonsai
{
    public class TerrainObject : MonoBehaviour, IDamagable
    {
        [SerializeField] public TerrainData.TerrainItemEnum terrainItemEnum;
        [SerializeField] public TerrainData.TerrainItemStruct terrainItemStruct;
        [SerializeField] public GameObject healthBar;
        [SerializeField] public GameObject healthTotalBar;
        [SerializeField] public GameObject healRemainingBar;
        [SerializeField] public GameObject labelPosition_go;
        [SerializeField] public TextMeshProUGUI healthText;

        [HideInInspector] GameObject itemSpawns;
        [HideInInspector] Camera playerCamera;
        [HideInInspector] string terrainItemName;
        [HideInInspector] float healthbarStartWidth;
        [HideInInspector] float currentHealth;
        [HideInInspector] float timeHealthbarVisible;
        [HideInInspector] float timeHealthbarVisibleRemaining;

        [SerializeField]
        private ItemEnum[] ItemsCanDamage;
        public ItemEnum[] itemsCanDamage { get => ItemsCanDamage; set { ItemsCanDamage = value; } }

        void Start()
        {
            terrainItemStruct = TerrainData.terrainItemDict[terrainItemEnum];
            terrainItemName = terrainItemStruct.baseName;
            itemSpawns = Core.FindGameObjectByNameAndTag("ItemSpawns", "ItemSpawns");
            playerCamera = Player.Instance.playerCamera.GetComponent<Camera>();

            Vector3 screenPoint = playerCamera.WorldToScreenPoint(labelPosition_go.transform.position);
            healthBar.transform.position = screenPoint;
            healthbarStartWidth = healRemainingBar.GetComponent<RectTransform>().sizeDelta.x;
            currentHealth = terrainItemStruct.health;
            timeHealthbarVisible = 5f;
            timeHealthbarVisibleRemaining = 0f;
        }

        private void Update()
        {
            UpdateHealthbar();
        }

        private void OnGUI()
        {
            if (timeHealthbarVisibleRemaining >= 0f)
            {
                PositionHealthLabel();
            }
        }

        public void UpdateHealthbar()
        {
            // Update health bar
            if (healthBar.activeSelf)
            {
                float healthbarWidth = (currentHealth / terrainItemStruct.health) * healthbarStartWidth;
                healRemainingBar.GetComponent<RectTransform>().sizeDelta = new Vector2(healthbarWidth, healRemainingBar.GetComponent<RectTransform>().sizeDelta.y);
                healthText.text = currentHealth.ToString() + "/" + terrainItemStruct.health.ToString();
            }

            if (timeHealthbarVisibleRemaining >= 0f)
            {
                timeHealthbarVisibleRemaining -= Time.deltaTime;
            }
            else
            {
                healthBar.SetActive(false);
            }
        }

        public void PositionHealthLabel()
        {
            Vector3 screenPoint = playerCamera.WorldToScreenPoint(labelPosition_go.transform.position);
            healthBar.transform.position = screenPoint;
            if (screenPoint.z < 0)
            {
                healthBar.SetActive(false);
            }
            else if (screenPoint.z >= 0 && !healthBar.activeSelf)
            {
                healthBar.SetActive(true);
            }
        }

        public void DestroyObject()
        {
            Vector3 terrainObjectPosition = this.gameObject.transform.position;
            Destroy(this.transform.parent.gameObject);

            // Spawn resources
            ItemData.ItemEnum itemEnum = terrainItemStruct.resourceType;
            GameObject resource = Resources.Load<GameObject>(GlobalData.itemsPrefabPath + ItemData.itemDict[itemEnum].modelRef);
            GameObject resourceInst = Instantiate(resource, itemSpawns.transform);
            resourceInst.transform.position = terrainObjectPosition;
            resourceInst.GetComponent<Item>().SetNumItem(terrainItemStruct.numResources);
            resourceInst.GetComponent<Rigidbody>().velocity = new Vector3(UnityEngine.Random.Range(-1f, 1f),
                                                              UnityEngine.Random.Range(4f, 5f),
                                                              UnityEngine.Random.Range(-1f, 1f));
        }

        public float Damage(float damage)
        {
            GameObject particleHit = Resources.Load<GameObject>(GlobalData.particlesPrefabPath + this.terrainItemStruct.particleHitName);
            GameObject particleSystem_go = Instantiate(particleHit, playerCamera.transform);
            particleSystem_go.transform.localPosition = Player.Instance.weaponCollider.center;

            currentHealth -= damage;
            PositionHealthLabel();
            timeHealthbarVisibleRemaining = timeHealthbarVisible;
            if (currentHealth <= 0f)
            {
                DestroyObject();
                return 0f;
            }

            return currentHealth;

        }

        public bool CanDamage(ItemData.ItemEnum itemEnum)
        {
            return itemsCanDamage.Contains(itemEnum);
        }
    }
}
