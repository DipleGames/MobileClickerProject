using UnityEngine;

[CreateAssetMenu(menuName = "Skill/Effects/Damage/ThunderBolt")]
public class ThunderBolt : SkillEffect
{
    public override void Execute(SkillRuntime skill)
    {
        Debug.Log("썬더 볼트");
    }
}
