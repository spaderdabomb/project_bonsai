using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public static class GlobalData
{
    public static int numLevels = new int();

    public static KeyCode default_forward = KeyCode.R;


    static GlobalData()
    {
        numLevels = 100;
    }

}

public static class SettingsData
{
    public static Dictionary<KeyBindType, Tuple<KeyCode, KeyCode>> defaultKeybinds;
    public static KeyBindType[] inventoryKeyBindTypes;

    static SettingsData()
    {
        defaultKeybinds = new Dictionary<KeyBindType, Tuple<KeyCode, KeyCode>>()
        {
            { KeyBindType.Forward, new Tuple<KeyCode, KeyCode> (KeyCode.W, KeyCode.None)},
            { KeyBindType.Back, new Tuple<KeyCode, KeyCode> (KeyCode.S, KeyCode.None) },
            { KeyBindType.Left, new Tuple<KeyCode, KeyCode> (KeyCode.A, KeyCode.None)},
            { KeyBindType.Right, new Tuple<KeyCode, KeyCode> (KeyCode.D, KeyCode.None)},
            { KeyBindType.Sprint, new Tuple<KeyCode, KeyCode> (KeyCode.LeftShift, KeyCode.None)},
            { KeyBindType.Interact, new Tuple<KeyCode, KeyCode> (KeyCode.E, KeyCode.None) },
            { KeyBindType.Drop, new Tuple<KeyCode, KeyCode> (KeyCode.Q, KeyCode.None)},
            { KeyBindType.Throw, new Tuple<KeyCode, KeyCode> (KeyCode.F, KeyCode.None) },
            { KeyBindType.ShowInventory, new Tuple<KeyCode, KeyCode> (KeyCode.Tab, KeyCode.None) },
            { KeyBindType.Inventory1, new Tuple<KeyCode, KeyCode> (KeyCode.Alpha1, KeyCode.None) },
            { KeyBindType.Inventory2, new Tuple<KeyCode, KeyCode> (KeyCode.Alpha2, KeyCode.None) },
            { KeyBindType.Inventory3, new Tuple<KeyCode, KeyCode> (KeyCode.Alpha3, KeyCode.None) },
            { KeyBindType.Inventory4, new Tuple<KeyCode, KeyCode> (KeyCode.Alpha4, KeyCode.None) },
            { KeyBindType.Inventory5, new Tuple<KeyCode, KeyCode> (KeyCode.Alpha5, KeyCode.None) },
            { KeyBindType.Inventory6, new Tuple<KeyCode, KeyCode> (KeyCode.Alpha6, KeyCode.None) },
            { KeyBindType.Inventory7, new Tuple<KeyCode, KeyCode> (KeyCode.Alpha7, KeyCode.None) },
            { KeyBindType.Inventory8, new Tuple<KeyCode, KeyCode> (KeyCode.Alpha8, KeyCode.None) },
            { KeyBindType.Inventory9, new Tuple<KeyCode, KeyCode> (KeyCode.Alpha9, KeyCode.None) },
            { KeyBindType.Inventory10,new Tuple<KeyCode, KeyCode> (KeyCode.Alpha0, KeyCode.None) }
        };
        inventoryKeyBindTypes = new KeyBindType[] { KeyBindType.Inventory1, KeyBindType.Inventory2, KeyBindType.Inventory3,
                                                    KeyBindType.Inventory4, KeyBindType.Inventory5, KeyBindType.Inventory6,
                                                    KeyBindType.Inventory7, KeyBindType.Inventory8, KeyBindType.Inventory9,
                                                    KeyBindType.Inventory10,};
    }

    public enum KeyBindType
    {
        Forward,
        Back,
        Left,
        Right,
        Sprint,
        Interact,
        Drop,
        Throw,
        ShowInventory,
        Inventory1,
        Inventory2,
        Inventory3,
        Inventory4,
        Inventory5,
        Inventory6,
        Inventory7,
        Inventory8,
        Inventory9,
        Inventory10,
        Attack,
        Block,
    }
}

public static class ItemData
{
    // Tuple --> <name, UI icon ref, 3D model ref, quest item, consumable, type, durability, AI usable, animation, description> 
    public static Dictionary<ItemEnum, ItemStruct> itemDict;

    static ItemData()
    {
        // Item Data
        itemDict = new Dictionary<ItemEnum, ItemStruct>
        {
            { ItemEnum.LogNormal, new ItemStruct("LogNormal", "LogNormalUI", "LogNormal3D", "LogNormalAnim", false, false, ItemType.Material, 1f, -1f, -1f, false, "Its a normal log") },
            { ItemEnum.Rock, new ItemStruct("Rock", "RockUI", "Rock3D", "RockNormalAnim", false, false, ItemType.Material, 1f, -1f, -1f, false, "Its a rock") },
            { ItemEnum.AxeStone, new ItemStruct("AxeStone", "AxeStoneUI", "AxeStone3D", "AxeStoneAnim", false, false, ItemType.Tool, 5f, -1f, 100f, false, "Its a stone axe") }
        };
    }

    public enum ItemEnum
    {
        LogNormal,
        Rock,
        AxeStone
    }

    public enum ItemType
    {
        Weapon,
        Offhand,
        Head,
        Ring,
        Necklace,
        Helmet,
        Chestplate,
        Leggings,
        Boots,
        Gloves,
        Food,
        Drink,
        Potion,
        Material,
        Tool
    }

    public struct ItemStruct
    {
        public string name;
        public string uiIconRef;
        public string modelRef;
        public string animation;
        public bool questItem;
        public bool consumable;
        public ItemType itemType;
        public float damage;
        public float armor;
        public float durability;
        public bool aiUsable;
        public string description;

        public ItemStruct(string _name, string _uiIconRef, string _modelRef, string _animation, bool _questItem, bool _consumable, ItemType _itemType,
                          float _damage, float _armor, float _durability, bool _aiUsable, string _description)
        {
            name = _name;
            uiIconRef = _uiIconRef;
            modelRef = _modelRef;
            animation = _animation;
            questItem = _questItem;
            consumable = _consumable;
            itemType = _itemType;
            damage = _damage;
            armor = _armor;
            durability = _durability;
            aiUsable = _aiUsable;
            description = _description;
        }
    }
}
