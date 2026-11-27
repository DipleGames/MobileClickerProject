using System;
using UnityEngine;

[Serializable]
public class SkillRuntime
{
    [SerializeField] private SkillConfig _config;
    [SerializeField] private SkillData _data;
    public SkillConfig Config => _config;
    public SkillData Data => _data;

    public float RemainingCoolTime { get; private set; }

    public SkillRuntime(SkillConfig config, SkillData data)
    {
        _config = config;
        _data = data;
    }

    public bool CanUse()
    {
        return Data.isUnlocked && RemainingCoolTime <= 0f;
    }

    /// <summary>
    /// 사용 가능한 스킬의 쿨타임을 시작하고 사용 성공 여부를 반환한다.
    /// </summary>
    public bool TryUse()
    {
        if (!CanUse())
        {
            return false;
        }

        RemainingCoolTime = Config.skillCoolTime;
        return true;
    }
    public void Tick(float deltaTime)
    {
        if (RemainingCoolTime > 0f)
        {
            RemainingCoolTime = System.Math.Max(0f, RemainingCoolTime - deltaTime);
        }
    }
}