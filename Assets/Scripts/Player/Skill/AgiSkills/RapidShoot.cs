//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class RapidShoot : SkillData
//{
//    [SerializeField] private float increaseAmountPercentage;
//    [SerializeField] private BasicAttackController bac;
//    [SerializeField] private float continuesTime;

//    private float _cachedAttackSpeed;

//    private void Start()
//    {
//        OnSkillEnd += ResetAdditionalAttackSpeed;
//        OnSkillStart += IncreaseAdditionalAttackSpeed;
//    }
//    public override void UseSkill()
//    {
//        Debug.Log("2");
//        base.UseSkill();
//        Debug.Log("3");

//        // StartCoroutine(nameof(InvokeSkillEnd));
//        Debug.Log("4");

//    }
//    private IEnumerator InvokeSkillEnd()
//    {
//        yield return new WaitForSeconds(continuesTime);
//        OnSkillEnd?.Invoke();
//    }
//    private void ResetAdditionalAttackSpeed()
//    {
//        Debug.Log("5");

//        bac.AdditionalAttackSpeed = 0;
//    }
//    private void IncreaseAdditionalAttackSpeed()
//    {
//        Debug.Log("6");

//        bac.AdditionalAttackSpeed += bac.AttackSpeed * (increaseAmountPercentage / 100);
//    }
//}
