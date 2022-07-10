using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Skill
{
    private SkillController Controller = null;
    private TargetController TargetController = null;

    public SkillData SkillData;

    public void SetController(Transform skillsParent)
    {
        TargetController = skillsParent.GetComponent<TargetController>();
        Type controllerType = SkillData.GetSkillControllerType();
        if (skillsParent.TryGetComponent(out SkillController controller))
        {
            Controller = controller;
        }
        else
        {
            Controller = (SkillController)skillsParent.gameObject.AddComponent(controllerType);
            Controller.SetupSkill(SkillData);
        }
    }
    public void SkillStart() { Controller.OnSkillStart(); }
    public void SkillEnd() { Controller.OnSkillEnd(); }
}
public abstract class SkillData : ScriptableObject
{
    public abstract Type GetSkillControllerType();
}
public abstract class SkillController : MonoBehaviour, ISkillController
{
    public abstract void SetupSkill(SkillData skillData);
    public abstract void OnSkillStart();
    public abstract void OnSkillEnd();

}


public interface ISkillController
{
    void OnSkillStart();
    void OnSkillEnd();
}
