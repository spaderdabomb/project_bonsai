using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using Unity.VisualScripting.FullSerializer;

public static class GlobalData
{
    public static int numLevels = new int();

    public static KeyCode default_forward = KeyCode.R;

    public static string uiPrefabPath;
    public static string itemsPrefabPath;
    public static string uiSpritesItemsPath;
    public static string uiIconsPSDPath;
    public static string uiItemsPSDPath;
    public static string uiMenusPSDPath;

    static GlobalData()
    {
        numLevels = 100;

        uiPrefabPath = "Prefabs/UI/";
        itemsPrefabPath = "Prefabs/Items/";
        uiSpritesItemsPath = "Sprites/UI/Items/";
        uiIconsPSDPath = "Photoshop/UIIcons/";
        uiItemsPSDPath = "Photoshop/UIItems/";
        uiMenusPSDPath = "Photoshop/UIMenus/";
    }

}

public static class SettingsData
{
    public static Dictionary<KeyBindType, Tuple<KeyCode, KeyCode>> defaultKeybinds;
    public static KeyBindType[] inventoryKeyBindTypes;
    public static Dictionary<KeyCode, string> keyNames;

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
            { KeyBindType.Inventory9, new Tuple<KeyCode, KeyCode> (KeyCode.Alpha9, KeyCode.None) }
        };

        inventoryKeyBindTypes = new KeyBindType[] { KeyBindType.Inventory1, KeyBindType.Inventory2, KeyBindType.Inventory3,
                                                    KeyBindType.Inventory4, KeyBindType.Inventory5, KeyBindType.Inventory6,
                                                    KeyBindType.Inventory7, KeyBindType.Inventory8, KeyBindType.Inventory9};

        // Keyname mapping from KeyCode to 'nice' string
        keyNames = new Dictionary<KeyCode, string>();
        foreach (KeyCode k in Enum.GetValues(typeof(KeyCode)))
        {
            try
            {
                keyNames.Add(k, k.ToString());
            }
            catch(ArgumentException e)
            {
                // Happens if key is already added, not an issue
                Console.WriteLine(e.Message);
            }
        }
        for (int i = 0; i < 10; i++)
        {
            keyNames[(KeyCode)((int)KeyCode.Alpha0 + i)] = i.ToString();
            keyNames[(KeyCode)((int)KeyCode.Keypad0 + i)] = i.ToString();
        }
        keyNames[KeyCode.Comma] = ",";
        keyNames[KeyCode.Escape] = "Esc";
        keyNames[KeyCode.None] = "-";
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
        Inventory9
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
            { ItemEnum.LogNormal, new ItemStruct("LogNormal", "LogNormalUI", "LogNormal3D", "LogNormalAnim", false, false, false, -1, ItemType.Material, 1f, -1f, false, "Its a normal log") },
            { ItemEnum.Rock, new ItemStruct("Rock", "RockUI", "Rock3D", "RockNormalAnim", false, false, false, -1, ItemType.Material, 1f, -1f, false, "Its a rock") },
            { ItemEnum.AxeStone, new ItemStruct("Stone Axe", "AxeStoneUI", "AxeStone3D", "AxeStoneAnim", false, false, true, 0, ItemType.Tool, 5f, -1f, false, "A not so sturdy stone axe") },
            { ItemEnum.HarpoonStone, new ItemStruct("Stone Harpoon", "HarpoonStoneUI", "HarpoonStone3D", "HarpoonStoneAnim", false, false, true, 0, ItemType.Tool, 5f, -1f, false, "A not so sturdy stone harpoon") },
            { ItemEnum.SpearStone, new ItemStruct("Stone Spear", "SpearStoneUI", "SpearStone3D", "SpearStoneAnim", false, false, true, 0, ItemType.Weapon, 10f, -1f, false, "A not so sturdy stone spear") },
            { ItemEnum.Compost, new ItemStruct("Compost", "CompostUI", "Compost3D", "CompostAnim", false, false, true, 0, ItemType.Material, -1f, -1f, false, "Gives a small boost to your plants") },
            { ItemEnum.ScrollBlank, new ItemStruct("Blank Scroll", "ScrollBlankUI", "ScrollBlank3D", "ScrollBlankAnim", false, false, true, 0, ItemType.Material, -1f, -1f, false, "A blank scroll for whatever your heart desires") }
        };
    }

    public enum ItemEnum
    {
        LogNormal,
        Rock,
        AxeStone,
        HarpoonStone,
        SpearStone,
        Compost,
        ScrollBlank
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
        public bool craftable;
        public int craftingMenuIdx;
        public ItemType itemType;
        public float damage;
        public float armor;
        public bool aiUsable;
        public string description;

        public ItemStruct(string _name, string _uiIconRef, string _modelRef, string _animation, bool _questItem, bool _consumable, bool _craftable, int _craftingMenuIdx, 
                          ItemType _itemType, float _damage, float _armor, bool _aiUsable, string _description)
        {
            name = _name;
            uiIconRef = _uiIconRef;
            modelRef = _modelRef;
            animation = _animation;
            questItem = _questItem;
            consumable = _consumable;
            craftable = _craftable;
            craftingMenuIdx = _craftingMenuIdx;
            itemType = _itemType;
            damage = _damage;
            armor = _armor;
            aiUsable = _aiUsable;
            description = _description;
        }
    }
}
