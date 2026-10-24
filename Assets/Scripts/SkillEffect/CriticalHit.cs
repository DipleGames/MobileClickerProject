using UnityEngine;

[CreateAssetMenu(menuName = "Skill/Effects/Damage")]
public class CriticalHit : SkillEffect
{
    public override void Execute(SkillRuntime skill)
    {
        Debug.Log("크리티클 히트");
    }
}
