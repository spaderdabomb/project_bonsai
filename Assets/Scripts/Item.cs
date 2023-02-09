using ProjectBonsai.StateMachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] public ItemData.ItemEnum itemEnum;
    [SerializeField] public ItemData.ItemStruct itemStruct;
    [SerializeField] public int itemQuantity = 1;

    private string itemName;

    void Start()
    {
        itemStruct = ItemData.itemDict[itemEnum];
        itemName = itemStruct.name;
    }

    public void SetNumItem(int _itemQuantity)
    {
        itemQuantity = _itemQuantity;
    }

    void Update()
    {
        Animate();
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
