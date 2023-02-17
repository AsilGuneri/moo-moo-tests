using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillSelectionPanel : MonoBehaviour
{
    [SerializeField] RectTransform[] skillParents = new RectTransform[4];
    [SerializeField] SelectableUISkill UISkillPrefab;
    [SerializeField] GameObject panel;

    ClassSkillsPair classSkills = new ClassSkillsPair();
    Class characterClass;


    private void Start()
    {
        StartCoroutine(CacheClassSkills());
        //SetClassSkills();
    }

    public void TogglePanel()
    {
        panel.SetActive(!panel.activeInHierarchy);
    }
    private void SetClassSkills()
    {
        foreach (var skill in classSkills.AllClassSkills)
        {
            var selectableSkill = Instantiate(UISkillPrefab);
            selectableSkill.Setup(skill.SkillData);
            selectableSkill.transform.SetParent(skillParents[skill.SkillData.Grade]);
        }


    }
    //this is for test only, its not supposed to a void 
    private IEnumerator CacheClassSkills()
    {
        yield return new WaitForSeconds(3);
        characterClass = UnitManager.Instance.GetPlayerController().CharacterClass;
        classSkills = PlayerSkillsDatabase.instance.GetClassSkills(characterClass);
        SetClassSkills();
    }
}
