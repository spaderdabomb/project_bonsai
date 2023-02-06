using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Mining", menuName = "ScriptableObjects/Skills/Mining", order = 1)]
public class MiningSkill : PlayerSkill
{
    public List<TerrainData.TerrainItemEnum> NewMaterialUnlocks { get; private set; }


    public override void OnExpGained()
    {
        base.OnExpGained();
    }
}