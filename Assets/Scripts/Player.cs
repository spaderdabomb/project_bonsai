using ProjectBonsai;
using ProjectBonsai.Assets.Scripts.Controllers;
using System.Collections.Generic;
using UnityEngine;

using ProjectBonsai.Assets.Scripts.UI;


public class Player : MonoBehaviour
{
    [SerializeField] GameObject interactPopup;
    [SerializeField] public GameObject playerCamera;
    [SerializeField] public CapsuleCollider weaponCollider;
    [SerializeField] public SphereCollider waterCollider;

    [SerializeField] public PlayerState CurrentPlayerState;
    [SerializeField] public PlayerCombatState CurrentPlayerCombatState;
    [SerializeField] public PlayerEnvironmentState CurrentPlayerEnvironmentState;

    [HideInInspector] public FirstPersonController fpController;
    [HideInInspector] public Rigidbody rb;

    private SphereCollider sphereCollider;
    private CapsuleCollider capsuleCollider;
    private List<Collider> triggerList = new List<Collider>();
    private List<Collider> colliderList = new List<Collider>();

    public PlayerData playerData;

    private void Awake()
    {

    }

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        fpController = gameObject.GetComponent<FirstPersonController>();
        sphereCollider = gameObject.GetComponent<SphereCollider>();
        capsuleCollider = gameObject.GetComponent<CapsuleCollider>();
        CurrentPlayerState = new PlayerState();
        CurrentPlayerState = PlayerState.Idling;
        CurrentPlayerCombatState = new PlayerCombatState();
        CurrentPlayerCombatState = PlayerCombatState.Idling;
        CurrentPlayerEnvironmentState = new PlayerEnvironmentState();
        CurrentPlayerEnvironmentState = PlayerEnvironmentState.OnLand;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnEnable()
    {
        // EventController.OnR += RTest;
    }

    private void OnDisable()
    {
        // EventController.OnR -= RTest;
    }
        
    public void Interact()
    {
        List<Collider> tempTriggerList = getTriggers();
        bool foundSubItem = false;
        foreach (Collider trigger in tempTriggerList)
        {
            if (trigger != null)
            {
                if (trigger.gameObject.CompareTag("SubItem") && !foundSubItem)
                {
                    // Get reference values
                    GameObject triggerParent = Core.GetFirstParentWithTag(trigger.gameObject, "Item");
                    Item triggerItem = triggerParent.GetComponent<Item>();
                    ItemData.ItemStruct itemStruct = triggerItem.itemStruct;
                    ItemData.ItemEnum itemEnum = triggerItem.itemEnum;
                    int itemQuantity = triggerItem.itemQuantity;

                    // Remove item model and add item UI
                    int leftOverItemQuantity = GameSceneController.Instance.AddUIItem(itemEnum, itemQuantity);
                    if (leftOverItemQuantity == 0)
                    {
                        Destroy(triggerParent);
                        triggerList.Remove(trigger);
                        interactPopup.GetComponent<InteractPopup>().HidePopup();
                    }
                    // When model remains, create new quantity
                    else
                    {
                        triggerParent.GetComponent<Item>().itemQuantity = leftOverItemQuantity;
                        interactPopup.GetComponent<InteractPopup>().ShowPopup(triggerParent);
                    }
                    foundSubItem = true;
                }
                else
                {
                    triggerList.Remove(trigger);
                }
            }
        }
    }

    public void StartAttack()
    {
        CurrentPlayerCombatState = PlayerCombatState.Attacking;
    }

    public void OnAttackHitFrame()
    {
        List<Collider> tempColliderList = weaponCollider.GetComponent<CollidersInTrigger>().GetList();

        foreach (Collider collider in tempColliderList)
        {
            if (collider == null)
            {
                weaponCollider.GetComponent<CollidersInTrigger>().RemoveItem(collider);
                continue;
            }

            if (!collider.TryGetComponent<IDamagable>(out var iDamagable))
            {
                print("does not have interface");
                continue;
            }

            ItemData.ItemEnum currentItemEnum = GameSceneController.Instance.toolHolder.itemGrid.GetComponent<ItemGrid>().GetCurrentSelectedItem();
            if (!iDamagable.CanDamage(currentItemEnum))
            {
                continue;
            }

            // TODO: include player base damage
            float healthRemaining = iDamagable.Damage(ItemData.weaponDict[currentItemEnum].attackBonus * 4f);
            if (healthRemaining <= 0f)
            {
                weaponCollider.GetComponent<CollidersInTrigger>().RemoveItem(collider);
            }
        }
    }

    // Searches toolholder than inventory for available slot to add item
    public (GameObject itemGrid, int itemIndex) GetIndexToAddItem(ItemData.ItemEnum itemEnum)
    {
        int indexToAdd;
        GameObject itemGrid;
        bool itemExistsInToolholder = GameSceneController.Instance.toolHolder.itemGrid.GetComponent<ItemGrid>().DoesNonMaxItemStackExist(itemEnum);
        bool itemExistsInInventory = GameSceneController.Instance.inventory.GetComponent<Inventory>().itemGrid.GetComponent<ItemGrid>().DoesNonMaxItemStackExist(itemEnum);
        if (itemExistsInToolholder)
        {
            indexToAdd = GameSceneController.Instance.toolHolder.itemGrid.GetComponent<ItemGrid>().GetFirstNonMaxStackSlotWithItem(itemEnum);
            itemGrid = GameSceneController.Instance.toolHolder.GetComponent<ToolHolder>().itemGrid;
        }
        else if (itemExistsInInventory)
        {
            indexToAdd = GameSceneController.Instance.inventory.GetComponent<Inventory>().itemGrid.GetComponent<ItemGrid>().GetFirstNonMaxStackSlotWithItem(itemEnum);
            itemGrid = GameSceneController.Instance.inventory.GetComponent<Inventory>().itemGrid;
        }
        else
        {
            indexToAdd = GameSceneController.Instance.toolHolder.itemGrid.GetComponent<ItemGrid>().GetFirstFreeSlotIndex();
            itemGrid = GameSceneController.Instance.toolHolder.itemGrid;
            if (indexToAdd == -1)
            {
                indexToAdd = GameSceneController.Instance.inventory.GetComponent<Inventory>().itemGrid.GetComponent<ItemGrid>().GetFirstFreeSlotIndex();
                itemGrid = GameSceneController.Instance.inventory.GetComponent<Inventory>().itemGrid;
            }
        }

        return (itemGrid, indexToAdd);
    }

    void OnTriggerEnter(Collider trigger)
    {
        if (!triggerList.Contains(trigger))
        {
            // NOTE: Must put on subitem trigger collider if you want to interact with item (Item layer doesn't interact with player)
            if (trigger.gameObject.CompareTag("SubItem"))
            {
                GameObject triggerParent = trigger.transform.parent.gameObject;
                if (triggerParent.CompareTag("Item"))
                {
                    interactPopup.GetComponent<InteractPopup>().ShowPopup(triggerParent);
                }
            }
            triggerList.Add(trigger);
        }
    }

    void OnTriggerExit(Collider trigger)
    {
        if (triggerList.Contains(trigger))
        {
            if (trigger.gameObject.CompareTag("SubItem"))
            {
                GameObject triggerParent = trigger.transform.parent.gameObject;
                if (triggerParent.CompareTag("Item"))
                {
                    interactPopup.GetComponent<InteractPopup>().HidePopup();
                }
            }
            triggerList.Remove(trigger);
        }
    }

    List<Collider> getTriggers()
    {
        List<Collider> triggers = new List<Collider>();
        foreach (Collider trigger in triggerList)
        {
            triggers.Add(trigger);
        }
        return triggers;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!colliderList.Contains(collision.collider))
        {
            colliderList.Add(collision.collider);
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (colliderList.Contains(collision.collider))
        {
            colliderList.Remove(collision.collider);
        }

    }


    List<Collider> getColliders()
    {
        List<Collider> colliders = new List<Collider>();
        foreach (Collider collider in colliderList)
        {
            colliders.Add(collider);
        }
        return colliders;
    }


    public enum PlayerState
    {
        Idling,
        Walking,
        Sprinting,
        Crouching,
        CrouchWalking,
        Jumping
    }

    public enum PlayerCombatState
    {
        Idling,
        Attacking,
        Blocking,
        Healing
    }

    public enum PlayerEnvironmentState
    {
        OnLand,
        InWater,
        OnIce
    }
}
