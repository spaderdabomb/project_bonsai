using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/Player", order = 1)]
public class PlayerData : ScriptableObject
{
    [field: Header("Damage Data")]
    [field: SerializeField] public float PlayerLevel { get; set; }
    [field: SerializeField] public float MatchLevel { get; set; }
    [field: SerializeField] public float HealthCurrent { get; set; }
    [field: SerializeField] public float HealthMax { get; set; }
    [field: SerializeField] public float ManaCurrent { get; set; }
    [field: SerializeField] public float ManaMax { get; set; }
    [field: SerializeField] public float SpeedCurrent { get; set; }
    [field: SerializeField] public float SpeedMax { get; set; }
    [field: SerializeField] public float AirCurrent { get; set; }
    [field: SerializeField] public float AirMax { get; set; }
    [field: SerializeField] public float StaminaCurrent { get; set; }
    [field: SerializeField] public float StaminaMax { get; set; }
    [field: SerializeField] public float JumpCurrent { get; set; }
    [field: SerializeField] public float JumpMax { get; set; }

    [field: Header("Damage Data")]

    [field: SerializeField] public float BaseDamage { get; set; }
    [field: SerializeField] public float BaseCritChance { get; set; }
    [field: SerializeField] public float BaseCritMultiplier { get; set; }
    [field: SerializeField] public float BaseDefense { get; set; }

    [Header("Level Data")]

    public WoodcuttingSkill WoodcuttingData;
    public FishingSkill FishingData;
    public MiningSkill MiningData;
}
