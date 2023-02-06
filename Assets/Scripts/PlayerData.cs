using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/Player", order = 1)]
public class PlayerData : ScriptableObject
{
    [Header("Player Attributes")]

    public float playerLevel;
    public float matchLevel;
    public float healthCurrent;
    public float healthMax;
    public float manaCurrent;
    public float manaMax;
    public float speedCurrent;
    public float speedMax;
    public float airCurrent;
    public float airMax;
    public float staminaCurrent;
    public float staminaMax;
    public float jumpCurrent;
    public float jumpMax;

    [Header("Damage Data")]

    public float baseDamage;
    public float baseCritChance;
    public float baseCritMultiplier;
    public float baseDefense;

    [Header("Level Data")]

    public float woodcuttingLevelMax;
    public float woodcuttingLevelCurrent;
    public float woodcuttingExp;
    public float harpooningLevelMax;
    public float harpooningLevelCurrent;
    public float harpooningExp;
    public float miningLevelMax;
    public float miningLevelCurrent;
    public float miningExp;
    public float cookingLevelMax;
    public float cookingLevelCurrent;
    public float cookingExp;
    public float farmingLevelMax;
    public float farmingLevelCurrent;
    public float farmingExp;
    public float staminaLevelMax;
    public float staminaLevelCurrent;
    public float staminaExp;
    public float stealthLevelMax;
    public float stealthLevelCurrent;
    public float stealthExp;
    public float meleeLevelMax;
    public float meleeLevelCurrent;
    public float meleeExp;
    public float rangeLevelMax;
    public float rangeLevelCurrent;
    public float rangeExp;
    public float magicLevelMax;
    public float magicLevelCurrent;
    public float magicExp;
    void Start()
    {
        
    }
}
