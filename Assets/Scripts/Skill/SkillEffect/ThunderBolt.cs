using UnityEngine;

[CreateAssetMenu(menuName = "Skill/Effects/Damage/ThunderBolt")]
public class ThunderBolt : SkillEffect
{
    public override void Execute(SkillRuntime skill)
    {
        double damage = skill.Config.skillMultiplier * AbilityManager.Instance.CurrentAbilityPower;
        Debug.Log($"{damage}");
        StageManager.Instance.CurrentMonster.TakeDamage(true, damage);
    }
}
