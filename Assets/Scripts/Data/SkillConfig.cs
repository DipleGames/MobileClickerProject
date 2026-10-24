using UnityEngine;

public enum SkillType
{
    Damage,
    Buff
}

[CreateAssetMenu(fileName = "SkillConfig", menuName = "Config/SkillConfig")]
public class SkillConfig : ScriptableObject
{
    public int skillID; // 스킬 id
    public SkillType skillType; // 스킬 타입
    public string skillName; // 스킬 이름
    public Sprite skillIcon; // 스킬 아이콘
    [TextArea(1,3)]
    public string skillDesc; // 스킬 설명

    public float skillMultiplier = 1.5f; // 스킬 주문력 배율
    public float skillCoolTime = 5f; // 스킬 쿨타임

    [Header("스킬 실행효과")]
    public SkillEffect skillEffect;
}