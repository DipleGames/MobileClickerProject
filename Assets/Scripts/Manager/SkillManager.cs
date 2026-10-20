using UnityEngine;

public class SkillManager : SingleTon<SkillManager>
{
    [SerializeField] private SkillConfig skillConfig;

    private SkillRuntime skill;

    private void Start()
    {
        SkillData data = new SkillData();

        // 게임 시작 시 한 번 생성
        skill = new SkillRuntime(skillConfig, data);
    }

    private void Update()
    {
        skill?.Tick(Time.deltaTime);
    }

    public void UseSkill()
    {
        skill?.Use();
    }
}