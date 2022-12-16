using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] public string itemName;
    [SerializeField] public ItemData.ItemEnum itemEnum;
    [SerializeField] public ItemData.ItemStruct itemStruct;
    void Start()
    {
        itemStruct = ItemData.itemDict[itemEnum];
        itemName = itemStruct.name;
    }

    protected virtual void InitializeEnemy()
    {

    }

    void Update()
    {

    }

    private void FixedUpdate()
    {

    }

    protected virtual void Attack()
    {
        
    }


    public enum ItemState
    {
        Idle
    }
}
