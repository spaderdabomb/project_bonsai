using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerSkill : ScriptableObject
{
    [field: SerializeField]
    public string SkillName { get; private set; }

    [field: SerializeField]
    public string SkillDescription { get; private set; }

    public string SkillLevel { get; private set; }
    public string SkillExp { get; private set; }

    public virtual void OnExpGained() { }
    public virtual void OnLevelUp() { }
}