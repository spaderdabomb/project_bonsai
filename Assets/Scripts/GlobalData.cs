using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public static class GlobalData
{
    public static int numLevels = new int();

    public static KeyCode default_forward = KeyCode.R;

    public static string uiPrefabPath;
    public static string uiItemsPrefabPath;
    public static string itemsPrefabPath;
    public static string uiSpritesItemsPath;
    public static string uiIconsPSDPath;
    public static string uiItemsPSDPath;
    public static string uiMenusPSDPath;

    public static string uiOrangeStandardHex;
    public static Color uiOrangeStandard;
    public static Color32 uiItemDisabledGray;

    static GlobalData()
    {
        numLevels = 100;

        uiPrefabPath = "Prefabs/UI/";
        uiItemsPrefabPath = "Prefabs/UI/ItemsUI/";
        itemsPrefabPath = "Prefabs/Items/";
        uiSpritesItemsPath = "Sprites/UI/Items/";
        uiIconsPSDPath = "Photoshop/UIIcons/";
        uiItemsPSDPath = "Photoshop/UIItems/";
        uiMenusPSDPath = "Photoshop/UIMenus/";

        uiOrangeStandardHex = "#D47f00";
        uiOrangeStandard = Core.ConvertHexColorToRGBA(uiOrangeStandardHex);
        uiItemDisabledGray = new Color32(100, 100, 100, 255);

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
    public static Dictionary<ItemEnum, CraftingRecipe> craftingRecipes;

    static ItemData()
    {
        // Item Data
        itemDict = new Dictionary<ItemEnum, ItemStruct>
        {
            { ItemEnum.Null, new ItemStruct("Null", "Null", "Null", "Null", false, false, 1, ItemType.Null, -1f, -1f, -1f, false, "Null") },
            { ItemEnum.LogNormal, new ItemStruct("Log", "LogNormalUI", "LogNormal3D", "LogNormalAnim", false, false, 50, ItemType.Material, 0.5f, -1f, -1f, false, "Its a normal log") },
            { ItemEnum.Rock, new ItemStruct("Rock", "RockUI", "Rock3D", "RockNormalAnim", false, false, 50, ItemType.Material, 1f, -1f, -1f, false, "Its a rock") },
            { ItemEnum.Rope, new ItemStruct("Rope", "RopeUI", "Rope3D", "RopeAnim", false, false, 50, ItemType.Material, 0.2f, -1f, -1f, false, "A nice cut of rope for tying things up") },
            { ItemEnum.Hemp, new ItemStruct("Hemp", "HempUI", "Hemp3D", "HempAnim", false, false, 100, ItemType.Material, 0.1f, -1f, -1f, false, "Its a rock") },
            { ItemEnum.AxeStone, new ItemStruct("Stone Axe", "AxeStoneUI", "AxeStone3D", "AxeStoneAnim", false, false, 1, ItemType.Tool, 2f, 5f, -1f, false, "A not so sturdy stone axe") },
            { ItemEnum.HarpoonStone, new ItemStruct("Stone Harpoon", "HarpoonStoneUI", "HarpoonStone3D", "HarpoonStoneAnim", false, false, 1, ItemType.Tool, 2f, 5f, -1f, false, "A not so sturdy stone harpoon") },
            { ItemEnum.SpearStone, new ItemStruct("Stone Spear", "SpearStoneUI", "SpearStone3D", "SpearStoneAnim", false, false, 1, ItemType.Weapon, 2f, 10f, -1f, false, "A not so sturdy stone spear") },
            { ItemEnum.Compost, new ItemStruct("Compost", "CompostUI", "Compost3D", "CompostAnim", false, false, 50, ItemType.Material, 1f, -1f, -1f, false, "Gives a small boost to your plants") },
            { ItemEnum.ScrollBlank, new ItemStruct("Blank Scroll", "ScrollBlankUI", "ScrollBlank3D", "ScrollBlankAnim", false, false, 100, ItemType.Material, 0.02f, -1f, -1f, false, "A blank scroll for whatever your heart desires") },
            { ItemEnum.BucketEmpty, new ItemStruct("Empty Bucket", "BucketEmptyUI", "BucketEmpty3D", "BucketEmptyAnim", false, false, 50, ItemType.Material, 0.05f, -1f, -1f, false, "An empty bucket, maybe you can put something in it?") }
        };

        // Crafting Recipe Data
        craftingRecipes = new Dictionary<ItemEnum, CraftingRecipe>
        {
            { ItemEnum.AxeStone, new CraftingRecipe(ItemEnum.LogNormal, 5, ItemEnum.Rock, 1, ItemEnum.Null, 0, ItemEnum.Null, 0, 0) },
            { ItemEnum.Rope, new CraftingRecipe(ItemEnum.Hemp, 2, ItemEnum.Null, 0, ItemEnum.Null, 0, ItemEnum.Null, 0, 0) },
            { ItemEnum.HarpoonStone, new CraftingRecipe(ItemEnum.LogNormal, 5, ItemEnum.Rock, 1, ItemEnum.Null, 0, ItemEnum.Null, 0, 0) },
            { ItemEnum.SpearStone, new CraftingRecipe(ItemEnum.LogNormal, 5, ItemEnum.Rock, 1, ItemEnum.Null, 0, ItemEnum.Null, 0, 0) },
            { ItemEnum.Compost, new CraftingRecipe(ItemEnum.LogNormal, 5, ItemEnum.Rock, 1, ItemEnum.Null, 0, ItemEnum.Null, 0, 0) },
            { ItemEnum.ScrollBlank, new CraftingRecipe(ItemEnum.LogNormal, 5, ItemEnum.Rock, 1, ItemEnum.Null, 0, ItemEnum.Null, 0, 0) },
            { ItemEnum.BucketEmpty, new CraftingRecipe(ItemEnum.LogNormal, 2, ItemEnum.Null, 0, ItemEnum.Null, 0, ItemEnum.Null, 0, 0) }
        };
    }

    public enum ItemEnum
    {
        Null,
        LogNormal,
        Rock,
        Rope,
        Hemp,
        AxeStone,
        HarpoonStone,
        SpearStone,
        Compost,
        ScrollBlank,
        BucketEmpty
    }

    public enum ItemType
    {
        Null,
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
        public int maxStack;
        public ItemType itemType;
        public float weight;
        public float damage;
        public float armor;
        public bool aiUsable;
        public string description;

        public ItemStruct(string _name, string _uiIconRef, string _modelRef, string _animation, bool _questItem, bool _consumable, int _maxStack, ItemType _itemType, 
                          float _weight, float _damage, float _armor, bool _aiUsable, string _description)
        {
            name = _name;
            uiIconRef = _uiIconRef;
            modelRef = _modelRef;
            animation = _animation;
            questItem = _questItem;
            consumable = _consumable;
            maxStack = _maxStack;
            itemType = _itemType;
            weight = _weight;
            damage = _damage;
            armor = _armor;
            aiUsable = _aiUsable;
            description = _description;
        }
    }

    public struct CraftingRecipe
    {
        public ItemEnum item1;
        public ItemEnum item2;
        public ItemEnum item3;
        public ItemEnum item4;
        public ItemEnum[] items;

        public int numItem1;
        public int numItem2;
        public int numItem3;
        public int numItem4;
        public int[] numItems;

        public int menuIndex;

        public CraftingRecipe(ItemEnum _item1, int _numItem1, ItemEnum _item2, int _numItem2, ItemEnum _item3, int _numItem3, ItemEnum _item4, int _numItem4, int _menuIndex)
        {
            item1 = _item1;
            item2 = _item2;
            item3 = _item3;
            item4 = _item4;
            items = new ItemEnum[4] { item1, item2, item3, item4 };
            
            numItem1 = _numItem1;
            numItem2 = _numItem2;
            numItem3 = _numItem3;   
            numItem4 = _numItem4;
            numItems = new int[4] {numItem1, numItem2, numItem3, numItem4 };

            menuIndex = _menuIndex;
        }
    }
}
