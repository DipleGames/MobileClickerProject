using UnityEngine;

[CreateAssetMenu(menuName = "Skill/Effects/Buff/AttackPowerIncrease")]
public class AttackPowerIncrease : SkillEffect
{
    [SerializeField, Min(0.01f)] private float _duration = 10f; // 공격력 버프 지속시간(초)
    [SerializeField] private float _multiplier = 2f;

    /// <summary>
    /// 스킬에 설정된 배율과 지속시간으로 공격력 버프를 적용한다.
    /// </summary>
    public override void Execute(SkillRuntime skill)
    {
        BuffManager.Instance.ApplyAttackBuff(_multiplier, _duration);
    }
}
