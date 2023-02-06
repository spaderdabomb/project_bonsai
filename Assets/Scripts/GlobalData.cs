using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;


#region Global Data

public static class GlobalData
{
    public static int numLevels = new int();

    public static KeyCode default_forward = KeyCode.R;

    public static string uiPrefabPath;
    public static string uiItemsPrefabPath;
    public static string equippedItemsPrefabPath;
    public static string itemsPrefabPath;
    public static string particlesPrefabPath;
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
        equippedItemsPrefabPath = "Prefabs/ItemsEquipped/";
        itemsPrefabPath = "Prefabs/Items/";
        particlesPrefabPath = "Prefabs/Particles/";
        uiSpritesItemsPath = "Sprites/UI/Items/";
        uiIconsPSDPath = "Photoshop/UIIcons/";
        uiItemsPSDPath = "Photoshop/UIItems/";
        uiMenusPSDPath = "Photoshop/UIMenus/";

        uiOrangeStandardHex = "#D47f00";
        uiOrangeStandard = Core.ConvertHexColorToRGBA(uiOrangeStandardHex);
        uiItemDisabledGray = new Color32(100, 100, 100, 255);

    }

}

#endregion

#region Settings Data


public static class SettingsData
{
    public static int[] keybindMenuLengths;
    public static Dictionary<KeyBindType, Tuple<KeyCode, KeyCode>> defaultKeybinds;
    public static KeyBindType[] inventoryKeyBindTypes;
    public static Dictionary<KeyCode, string> keyNames;

    static SettingsData()
    {
        keybindMenuLengths = new int[2] { 6, 14 };
        defaultKeybinds = new Dictionary<KeyBindType, Tuple<KeyCode, KeyCode>>()
        {
            { KeyBindType.Forward, new Tuple<KeyCode, KeyCode> (KeyCode.W, KeyCode.None)},
            { KeyBindType.Back, new Tuple<KeyCode, KeyCode> (KeyCode.S, KeyCode.None) },
            { KeyBindType.Left, new Tuple<KeyCode, KeyCode> (KeyCode.A, KeyCode.None)},
            { KeyBindType.Right, new Tuple<KeyCode, KeyCode> (KeyCode.D, KeyCode.None)},
            { KeyBindType.Sprint, new Tuple<KeyCode, KeyCode> (KeyCode.LeftShift, KeyCode.None)},
            { KeyBindType.Attack, new Tuple<KeyCode, KeyCode> (KeyCode.Mouse0, KeyCode.None)},
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
        Attack,
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

#endregion

#region Item Data

public static class ItemData
{
    public static Dictionary<ItemEnum, ItemStruct> itemDict;
    public static Dictionary<ItemEnum, WeaponStruct> weaponDict;
    public static Dictionary<ItemEnum, CraftingRecipe> craftingRecipes;

    static ItemData()
    {
        // Item Data
        itemDict = new Dictionary<ItemEnum, ItemStruct>
        {
            { ItemEnum.Null, new ItemStruct("Null", "Null", false, false, 1, ItemType.Null, -1f, -1f, false, "Null") },
            { ItemEnum.LogNormal, new ItemStruct("Log", "LogNormal", false, false, 50, ItemType.Material, 0.5f, -1f, false, "Its a normal log") },
            { ItemEnum.Rock, new ItemStruct("Rock", "Rock", false, false, 50, ItemType.Material, 1f, -1f, false, "Its a rock") },
            { ItemEnum.Rope, new ItemStruct("Rope", "Rope", false, false, 50, ItemType.Material, 0.2f, -1f, false, "A nice cut of rope for tying things up") },
            { ItemEnum.Hemp, new ItemStruct("Hemp", "Hemp", false, false, 100, ItemType.Material, 0.1f, -1f, false, "Its a rock") },
            { ItemEnum.AxeStone, new ItemStruct("Stone Axe", "AxeStone", false, false, 1, ItemType.Tool, 2f, -1f, false, "A not so sturdy stone axe") },
            { ItemEnum.HarpoonStone, new ItemStruct("Stone Harpoon", "HarpoonStone", false, false, 1, ItemType.Tool, 2f, -1f, false, "A not so sturdy stone harpoon") },
            { ItemEnum.PickaxeStone, new ItemStruct("Stone Pickaxe", "PickaxeStone", false, false, 1, ItemType.Tool, 2f, -1f, false, "A not so sturdy stone spear") },
            { ItemEnum.SpearStone, new ItemStruct("Stone Spear", "SpearStone", false, false, 1, ItemType.Weapon, 2f, -1f, false, "A not so sturdy stone spear") },
            { ItemEnum.Compost, new ItemStruct("Compost", "Compost", false, false, 50, ItemType.Material, 1f, -1f, false, "Gives a small boost to your plants") },
            { ItemEnum.ScrollBlank, new ItemStruct("Blank Scroll", "ScrollBlank", false, false, 100, ItemType.Material, 0.02f, -1f, false, "A blank scroll for whatever your heart desires") },
            { ItemEnum.BucketEmpty, new ItemStruct("Empty Bucket", "BucketEmpty", false, false, 50, ItemType.Material, 0.05f, -1f, false, "An empty bucket, maybe you should put something in it?") },
            { ItemEnum.OreCopper, new ItemStruct("Copper Ore", "OreCopper", false, false, 50, ItemType.Material, 2f, -1f, false, "A nice haul of copper") },
            { ItemEnum.OreTin, new ItemStruct("Tin Ore", "OreTin", false, false, 50, ItemType.Material, 2f, -1f, false, "A nice haul of Tin") },
            { ItemEnum.OreIron, new ItemStruct("Iron Ore", "OreIron", false, false, 50, ItemType.Material, 2f, -1f, false, "A nice haul of Iron") },
            { ItemEnum.OreCoal, new ItemStruct("Coal Ore", "OreCoal", false, false, 50, ItemType.Material, 2f, -1f, false, "A nice haul of Coal") },
            { ItemEnum.OreSilver, new ItemStruct("Silver Ore", "OreSilver", false, false, 50, ItemType.Material, 2f, -1f, false, "A nice haul of Silver") },
            { ItemEnum.OreGold, new ItemStruct("Gold Ore", "OreGold", false, false, 50, ItemType.Material, 2f, -1f, false, "A nice haul of Gold") },
            { ItemEnum.OreTitanium, new ItemStruct("Titanium Ore", "OreTitanium", false, false, 50, ItemType.Material, 2f, -1f, false, "A nice haul of Titanium") },
            { ItemEnum.HideBoar, new ItemStruct("Boar Hide", "HideBoar", false, false, 20, ItemType.Material, 5f, -1f, false, "A boar hide") },
            { ItemEnum.MeatBoarRaw, new ItemStruct("Raw Boar Meat", "MeatBoarRaw", false, false, 50, ItemType.Material, 1f, -1f, false, "A nice slice of raw boar meat") },
            { ItemEnum.MeatBoar, new ItemStruct("Raw Boar Meat", "MeatBoar", false, true, 50, ItemType.Material, 1f, -1f, false, "A nice slice of boar meat") },
            { ItemEnum.AnyWeapon, new ItemStruct("Null", "Null", false, false, 1, ItemType.Weapon, -1f, -1f, false, "Null") }
        };

        weaponDict = new Dictionary<ItemEnum, WeaponStruct>
        {
            { ItemEnum.Null, new WeaponStruct("Null", "Null", -1f, -1f, -1f, -1f) },
            { ItemEnum.AxeStone, new WeaponStruct("Stone Axe", "AxeStone", 0f, 5f, 0f, 0f) },
            { ItemEnum.PickaxeStone, new WeaponStruct("Stone Pickaxe", "PickaxeStone", 0f, 5f, 0f, 0f) },
            { ItemEnum.AnyWeapon, new WeaponStruct("Null", "Null", -1f, -1f, -1f, -1f) }
        };

        // Crafting Recipe Data
        craftingRecipes = new Dictionary<ItemEnum, CraftingRecipe>
        {
            { ItemEnum.AxeStone, new CraftingRecipe(ItemEnum.LogNormal, 5, ItemEnum.Rock, 1, ItemEnum.Null, 0, ItemEnum.Null, 0, 0) },
            { ItemEnum.Rope, new CraftingRecipe(ItemEnum.Hemp, 2, ItemEnum.Null, 0, ItemEnum.Null, 0, ItemEnum.Null, 0, 0) },
            { ItemEnum.HarpoonStone, new CraftingRecipe(ItemEnum.LogNormal, 5, ItemEnum.Rock, 1, ItemEnum.Null, 0, ItemEnum.Null, 0, 0) },
            { ItemEnum.PickaxeStone, new CraftingRecipe(ItemEnum.LogNormal, 5, ItemEnum.Rock, 1, ItemEnum.Null, 0, ItemEnum.Null, 0, 0) },
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
        PickaxeStone,
        SpearStone,
        Compost,
        ScrollBlank,
        BucketEmpty,
        OreCopper,
        OreTin,
        OreIron,
        OreCoal,
        OreSilver,
        OreGold,
        OreTitanium,
        HideBoar,
        MeatBoarRaw,
        MeatBoar,
        AnyWeapon,
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
        public string baseName;
        public string uiIconRef;
        public string modelRef;
        public string animation;
        public bool questItem;
        public bool consumable;
        public int maxStack;
        public ItemType itemType;
        public float weight;
        public float armor;
        public bool aiUsable;
        public string description;

        public ItemStruct(string _name, string _baseName, bool _questItem, bool _consumable, int _maxStack, ItemType _itemType, 
                          float _weight, float _armor, bool _aiUsable, string _description)
        {
            name = _name;
            baseName = _baseName;
            uiIconRef = _baseName + "UI";
            modelRef = _baseName + "/" + _baseName + "3D";
            animation = _baseName + "Anim";
            questItem = _questItem;
            consumable = _consumable;
            maxStack = _maxStack;
            itemType = _itemType;
            weight = _weight;
            armor = _armor;
            aiUsable = _aiUsable;
            description = _description;
        }
    }

    public struct WeaponStruct
    {
        public string name;
        public string baseName;
        public string animationName;
        public string equippedName;
        public float defenseBonus;
        public float attackBonus;
        public float healthBonus;
        public float manaBonus;

        public WeaponStruct(string _name, string _baseName, float _defenseBonus, float _attackBonus, float _healthBonus, float _manaBonus)
        {
            name = _name;
            baseName = _baseName;
            animationName = _baseName + "Anim";
            equippedName = _baseName + "Equipped";
            defenseBonus = _defenseBonus;
            attackBonus = _attackBonus;
            healthBonus = _healthBonus;
            manaBonus = _manaBonus;
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

#endregion

#region Terrain Data

public static class TerrainData
{
    public static Dictionary<TerrainItemEnum, TerrainItemStruct> terrainItemDict;

    static TerrainData()
    {
        terrainItemDict = new Dictionary<TerrainItemEnum, TerrainItemStruct>()
        {
            { TerrainItemEnum.Null, new TerrainItemStruct("Null", "Null", "Null", false, -1f, -1, ItemData.ItemEnum.Null)},
            { TerrainItemEnum.TreePine, new TerrainItemStruct("Tree", "TreeNormal", "Tree", true, 100f, 20, ItemData.ItemEnum.LogNormal)},
            { TerrainItemEnum.OreCopper, new TerrainItemStruct("Copper Ore", "OreCopper", "Ore", true, 100f, 5, ItemData.ItemEnum.OreCopper)},
            { TerrainItemEnum.OreTin, new TerrainItemStruct("Tin Ore", "OreTin", "Ore", true, 100f, 5, ItemData.ItemEnum.OreTin)},
            { TerrainItemEnum.OreIron, new TerrainItemStruct("Iron Ore", "OreIron", "Ore", true, 100f, 5, ItemData.ItemEnum.OreIron)},
            { TerrainItemEnum.OreCoal, new TerrainItemStruct("Coal Ore", "OreCoal", "Ore", true, 100f, 5, ItemData.ItemEnum.OreCoal)},
            { TerrainItemEnum.OreSilver, new TerrainItemStruct("Silver Ore", "OreSilver", "Ore", true, 100f, 5, ItemData.ItemEnum.OreSilver)},
            { TerrainItemEnum.OreGold, new TerrainItemStruct("Gold Ore", "OreGold", "Ore", true, 100f, 5, ItemData.ItemEnum.OreGold)},
            { TerrainItemEnum.OreTitanium, new TerrainItemStruct("Titanium Ore", "OreTitanium", "Ore", true, 100f, 5, ItemData.ItemEnum.OreTitanium)}
        };
    }

    public enum TerrainItemEnum
    {
        Null,
        TreePine,
        TreeDead,
        OreCopper,
        OreTin,
        OreIron,
        OreCoal,
        OreSilver,
        OreGold,
        OreTitanium
    }

    public struct TerrainItemStruct
    {
        public string name;
        public string baseName;
        public string tag;
        public string prefabName;
        public string animationName;
        public string particleHitName;
        public bool canBeDamaged;
        public float health;
        public int numResources;
        public ItemData.ItemEnum resourceType;

        public TerrainItemStruct(string _name, string _baseName, string _tag, bool _canBeDamaged, float _health, int _numResources, ItemData.ItemEnum _resourceType)
        {
            name = _name;
            baseName = _baseName;
            tag = _tag;
            prefabName = _baseName + "Prefab";
            animationName = _baseName + "Anim";
            particleHitName = _baseName + "Particles";
            canBeDamaged = _canBeDamaged;
            health = _health;
            numResources = _numResources;   
            resourceType= _resourceType;
        }
    }
}

#endregion

#region Objective Data

public static class ObjectiveData
{
    public static List<ObjectiveTask> objectiveDataList; // currently only has objectives for a single level

    static ObjectiveData()
    {
        // Single level objectives
        objectiveDataList = new List<ObjectiveTask>()
        {
            new ObjectiveTask(new List<ObjectiveSubTask>() 
            { 
                new SkillProgressionTask(typeof(WoodcuttingSkill), 5), new LandmarkTask(Vector3.zero, 10f) 
            })
        };
    }
}

#endregion


public static class SkillData
{
    static SkillData()
    {

    }

    public enum SkillType
    {
        Woodcutting,
        Mining,
        Fishing
    }
}