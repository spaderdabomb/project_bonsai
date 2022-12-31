using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] public string itemName;
    [SerializeField] public ItemData.ItemEnum itemEnum;
    [SerializeField] public ItemData.ItemStruct itemStruct;

    [SerializeField] public int itemQuantity = 1;

    public GameObject player;

    void Start()
    {
        itemStruct = ItemData.itemDict[itemEnum];
        itemName = itemStruct.name;
        player = GameObject.FindGameObjectWithTag("Player").gameObject;
    }

    public void SetNumItem(int _itemQuantity)
    {
        itemQuantity = _itemQuantity;
    }

    void Update()
    {

    }

    private void FixedUpdate()
    {

    }

    protected virtual void Animate()
    {

    }


    public enum ItemState
    {
        Idle
    }
}
