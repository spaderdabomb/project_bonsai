using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Woodcutting", menuName = "ScriptableObjects/Skills/Woodcutting", order = 1)]
public class WoodcuttingSkill : PlayerSkill
{
    public List<TerrainData.TerrainItemEnum> NewMaterialUnlocks { get; private set; }

    public override void OnExpGained()
    {
        base.OnExpGained();
    }
}