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
                    GameObject colliderParent = Core.GetFirstParentWithTag(collider.gameObject, "Item");
                    string itemName = colliderParent.GetComponent<Item>().itemStruct.uiIconRef;
                    Destroy(colliderParent);
                    triggerList.Remove(collider);
                    interactPopup.GetComponent<InteractPopup>().HidePopup();

                    int firstAvailableIndex = GameSceneController.Instance.toolHolder.GetComponent<ToolHolder>().itemGrid.GetComponent<ItemGrid>().GetFirstFreeSlotIndex();
                    GameSceneController.Instance.toolHolder.GetComponent<ToolHolder>().itemGrid.GetComponent<ItemGrid>().InstantiateItem(itemName, firstAvailableIndex);
                    foundSubItem = true;
                }
                else
                {
                    triggerList.Remove(collider);
                }
            }
        }

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
                    interactPopup.GetComponent<InteractPopup>().ShowPopup();
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
