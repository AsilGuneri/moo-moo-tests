using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillController : MonoBehaviour
{
    public List<Skill> possibleSkills;
    [SerializeField] private List<Skill> _selectedSkills = new List<Skill>();
    public Skill _skill;
    public List<Skill> SelectedSkills
    {
        get { return _selectedSkills; }
        set { _selectedSkills = value; }
    }

    public void UnlockSkill(Skill skill)
    {
        skill.SetController(transform);
        SelectedSkills.Add(skill);
        WaveManager.Instance.CurrentTier++;
        SkillSelectionPanel.Instance.ChangeCanvasEnabled(false);

    }
    public void UseSkill(int tier)
    {
        if (SelectedSkills[tier] != null) SelectedSkills[tier].SkillStart();
    }
}
