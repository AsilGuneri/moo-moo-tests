using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;
using UnityEngine.Events;

public class SkillSelectionPanel : Singleton<SkillSelectionPanel>
{
    [SerializeField] private List<SkillFieldUI> _skillFields;

    private Canvas _canvas;
    

    void Start()
    {
        _canvas = GetComponent<Canvas>();  
    }

    public void SetPanel(List<Skill> possibleSkills, PlayerSkillController psc)
    {
        _canvas.enabled = true;
        for (int i = 0; i < possibleSkills.Count; i++)
        {
            var skill = possibleSkills[i];
            _skillFields[i].SetSkillField(psc, skill);

        }
    }
    public void ChangeCanvasEnabled(bool isEnabled)
    {
        _canvas.enabled = isEnabled;
    }
}
