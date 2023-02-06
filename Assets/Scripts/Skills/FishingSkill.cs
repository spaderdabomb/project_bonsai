using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Fishing", menuName = "ScriptableObjects/Skills/Fishing", order = 1)]
public class FishingSkill : PlayerSkill
{
    public List<AiType.AiEnum> NewAIUnlocks { get; private set; }

    public override void OnExpGained()
    {
        base.OnExpGained();
    }
}