using ProjectBonsai.Assets.Scripts.Controllers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ObjectiveTask
{
    public string ObjectiveName { get; private set; }
    public string ObjectiveDescription { get; private set; }
    public List<ObjectiveSubTask> ObjectiveSubTasks { get; private set; } 

    public ObjectiveTask(List<ObjectiveSubTask> objectiveSubTasks)
    {
        ObjectiveSubTasks = objectiveSubTasks;
    }

    public virtual void OnTaskStart() { }
    public virtual void OnTaskFinish() { }
    public virtual bool IsTaskCompleted() { return false; }
}

public abstract class ObjectiveSubTask
{
    public string ObjectiveDescription { get; private set; }

    public virtual void OnSubTaskStart() { }
    public virtual void OnSubTaskFinish() { }
    public virtual bool IsSubTaskCompleted() { return false; }
}

public class SkillProgressionTask : ObjectiveSubTask
{
    public Type SkillType { get; private set; }
    public string SkillDisplayName { get; private set; }
    public int TargetLevel { get; private set; }

    public SkillProgressionTask(Type skillType, int targetLevel)
    {
        SkillType = skillType;
        TargetLevel = targetLevel;
        SkillDisplayName = GameSceneController.Instance.skillDict[SkillData.SkillType.Woodcutting].SkillName;
        Debug.Log(SkillDisplayName);
    }

    public override void OnSubTaskStart()
    {
        base.OnSubTaskStart();
        Debug.Log("task started");
        Debug.Log("task started"+this.SkillType.ToString()+this.TargetLevel.ToString());
    }
}

public class LandmarkTask : ObjectiveSubTask
{
    public Vector3 TargetPosition { get; private set; }
    public float Distance { get; private set; }

    public LandmarkTask(Vector3 targetPosition, float distance)
    {
        TargetPosition = targetPosition;
        Distance = distance;
    }

    public override void OnSubTaskStart()
    {
        // do smth
    }
}