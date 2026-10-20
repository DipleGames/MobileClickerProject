using UnityEngine;

[CreateAssetMenu(fileName = "SkillConfig", menuName = "Config/SkillConfig")]
public class SkillConfig : ScriptableObject
{
    public string skillName;
    public Sprite skillIcon;

    public float skillMultiplier = 1.5f;
    public float skillCoolTime = 5f;
}