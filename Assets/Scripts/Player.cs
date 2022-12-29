using ProjectBonsai;
using ProjectBonsai.Assets.Scripts.Controllers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] GameObject interactPopup;

    Rigidbody rb;
    SphereCollider sphereCollider;
    CapsuleCollider capsuleCollider;
    List<Collider> triggerList = new List<Collider>();

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        sphereCollider = gameObject.GetComponent<SphereCollider>();
        capsuleCollider = gameObject.GetComponent<CapsuleCollider>();
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
        List<Collider> colliderList = getColliders();
        bool foundSubItem = false;
        foreach (Collider collider in colliderList)
        {
            if (collider != null)
            {
                if (collider.gameObject.tag == "SubItem" && !foundSubItem)
                {
                    // Get reference values
                    GameObject colliderParent = Core.GetFirstParentWithTag(collider.gameObject, "Item");
                    Item colliderItem = colliderParent.GetComponent<Item>();
                    ItemData.ItemStruct itemStruct = colliderItem.itemStruct;
                    ItemData.ItemEnum itemEnum = colliderItem.itemEnum;
                    int itemQuantity = colliderItem.itemQuantity;

                    // Remove item model and add item UI
                    int leftOverItemQuantity = GameSceneController.Instance.AddUIItem(itemEnum, itemQuantity);
                    if (leftOverItemQuantity == 0)
                    {
                        Destroy(colliderParent);
                        triggerList.Remove(collider);
                        interactPopup.GetComponent<InteractPopup>().HidePopup();
                    }
                    // When model remains, create new quantity
                    else
                    {
                        colliderParent.GetComponent<Item>().itemQuantity = leftOverItemQuantity;
                        interactPopup.GetComponent<InteractPopup>().ShowPopup(colliderParent);
                    }
                    foundSubItem = true;
                }
                else
                {
                    triggerList.Remove(collider);
                }
            }
        }

    }

    // Searches toolholder than inventory for available slot to add item
    public (GameObject itemGrid, int itemIndex) GetIndexToAddItem(ItemData.ItemEnum itemEnum)
    {
        int indexToAdd;
        GameObject itemGrid;
        bool itemExistsInToolholder = GameSceneController.Instance.toolHolder.GetComponent<ToolHolder>().itemGrid.GetComponent<ItemGrid>().DoesNonMaxItemStackExist(itemEnum);
        bool itemExistsInInventory = GameSceneController.Instance.inventory.GetComponent<Inventory>().itemGrid.GetComponent<ItemGrid>().DoesNonMaxItemStackExist(itemEnum);
        if (itemExistsInToolholder)
        {
            indexToAdd = GameSceneController.Instance.toolHolder.GetComponent<ToolHolder>().itemGrid.GetComponent<ItemGrid>().GetFirstNonMaxStackSlotWithItem(itemEnum);
            itemGrid = GameSceneController.Instance.toolHolder.GetComponent<ToolHolder>().itemGrid;
        }
        else if (itemExistsInInventory)
        {
            indexToAdd = GameSceneController.Instance.inventory.GetComponent<Inventory>().itemGrid.GetComponent<ItemGrid>().GetFirstNonMaxStackSlotWithItem(itemEnum);
            itemGrid = GameSceneController.Instance.inventory.GetComponent<Inventory>().itemGrid;
        }
        else
        {
            indexToAdd = GameSceneController.Instance.toolHolder.GetComponent<ToolHolder>().itemGrid.GetComponent<ItemGrid>().GetFirstFreeSlotIndex();
            itemGrid = GameSceneController.Instance.toolHolder.GetComponent<ToolHolder>().itemGrid;
            if (indexToAdd == -1)
            {
                indexToAdd = GameSceneController.Instance.inventory.GetComponent<Inventory>().itemGrid.GetComponent<ItemGrid>().GetFirstFreeSlotIndex();
                itemGrid = GameSceneController.Instance.inventory.GetComponent<Inventory>().itemGrid;
            }
        }

        return (itemGrid, indexToAdd);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!triggerList.Contains(other))
        {
            if (other.gameObject.tag == "SubItem")
            {
                GameObject colliderParent = other.transform.parent.gameObject;
                if (colliderParent.tag == "Item")
                {
                    interactPopup.GetComponent<InteractPopup>().ShowPopup(colliderParent);
                }
            }
            triggerList.Add(other);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (triggerList.Contains(other))
        {
            if (other.gameObject.tag == "SubItem")
            {
                GameObject colliderParent = other.transform.parent.gameObject;
                if (colliderParent.tag == "Item")
                {
                    interactPopup.GetComponent<InteractPopup>().HidePopup();
                }
            }
            triggerList.Remove(other);
        }
    }

    List<Collider> getColliders()
    {
        List<Collider> colliders = new List<Collider>();
        foreach (Collider col in triggerList)
        {
            colliders.Add(col);
        }
        return colliders;
    }


}
