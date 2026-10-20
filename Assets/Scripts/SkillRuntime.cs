public class SkillRuntime
{
    public SkillConfig Config { get; private set; }
    public SkillData Data { get; private set; }

    public float RemainingCoolTime { get; private set; }

    public SkillRuntime(SkillConfig config, SkillData data)
    {
        Config = config;
        Data = data;
    }

    public bool CanUse()
    {
        return Data.isUnlocked && RemainingCoolTime <= 0f;
    }

    public void Use()
    {
        if (!CanUse())
            return;

        RemainingCoolTime = Config.skillCoolTime;

        // 스킬 효과 실행
    }

    public void Tick(float deltaTime)
    {
        if (RemainingCoolTime > 0f)
        {
            RemainingCoolTime = System.Math.Max(0f, RemainingCoolTime - deltaTime);
        }
    }
}