using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillController : MonoBehaviour
{
    [SerializeField] private List<Skill> _skills = new List<Skill>();
    private bool _isInitialized = false;
   
    public void InitializeSkills()
    {
        if (_isInitialized) return;
        foreach(var skill in _skills)
        {
            skill.SetController(transform);
        }
        _isInitialized = true;
    }
    public void UseSkill(int tier)
    {
        InitializeSkills();
        _skills[tier].SkillStart();
    }
}
