using ProjectBonsai.Assets.Scripts.Controllers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ObjectiveTask
{
    public string ObjectiveName { get; private set; }
    public List<ObjectiveSubTask> ObjectiveSubTasks { get; private set; }

    public ObjectiveTask(string objectiveName, List<ObjectiveSubTask> objectiveSubTasks)
    {
        ObjectiveName = objectiveName;
        ObjectiveSubTasks = objectiveSubTasks;
    }

    public void OnTaskStart() 
    {
        UIManager.Instance.objectivesUI.GetComponent<ObjectivesUI>().ClearUI();
        UIManager.Instance.objectivesUI.GetComponent<ObjectivesUI>().InitNewObjectiveUI(ObjectiveName, ObjectiveSubTasks);
    }

    public void OnTaskFinish() { }
    public bool IsTaskCompleted() { return false; }
}

public abstract class ObjectiveSubTask
{
    public virtual string ObjectiveDescription { get; set; }

    public virtual void OnSubTaskStart() { }
    public virtual void OnSubTaskFinish() { }
    public virtual bool IsSubTaskCompleted() { return false; }
}

public class SkillProgressionTask : ObjectiveSubTask
{
    public SkillData.SkillType SkillEnum { get; private set; }
    public int TargetLevel { get; private set; }

    public SkillProgressionTask(string objectiveDescription, SkillData.SkillType skillEnum, int targetLevel)
    {
        ObjectiveDescription = objectiveDescription;
        SkillEnum = skillEnum;
        TargetLevel = targetLevel;
        Debug.Log(Player.Instance.playerData.WoodcuttingData.SkillName);
    }

    public override void OnSubTaskStart()
    {
        base.OnSubTaskStart();
    }
}

public class LandmarkTask : ObjectiveSubTask
{
    public Vector3 TargetPosition { get; private set; }
    public float Distance { get; private set; }

    public LandmarkTask(string objectiveDescription, Vector3 targetPosition, float distance)
    {
        ObjectiveDescription = objectiveDescription;
        TargetPosition = targetPosition;
        Distance = distance;
    }

    public override void OnSubTaskStart()
    {
        // do smth
    }
}