using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class Burn : SkillEffect
{
    public float Duration;
    public int DamagePerSecond;
    public float DamageInterval;

    private bool isApplying = false;
    private float counter;
    private float intervalCounter;

    public override void Apply(UnitController source, UnitController target)
    {
        isApplying = true;
        counter = Duration;
        if (DamageInterval <= 0) DamageInterval = 1;
        ApplyOverTime(source, target);
    }
    private async Task ApplyOverTime(UnitController source ,UnitController target)
    {
        while(counter > 0)
        {
            target.Health.TakeDamage(DamagePerSecond, source.transform);
            await Task.Delay((int)(DamageInterval * 1000));
            counter -= DamageInterval;
        }
    }
}
