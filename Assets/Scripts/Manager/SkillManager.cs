using UnityEngine;
using System.Collections.Generic;

public class SkillManager : SingleTon<SkillManager>
{
    [SerializeField] private List<SkillConfig> _skillConfigs;
    private Dictionary<int, SkillRuntime> _skills = new();


    // 읽기전용
    public List<SkillConfig> SkillConfigs => _skillConfigs;
    public Dictionary<int, SkillRuntime> Skiils => _skills;


    public void InitializeSkills(List<SkillData> savedSkillDatas)
    {
        _skills.Clear();

        Dictionary<int, SkillData> savedDict = new();

        if (savedSkillDatas != null)
        {
            foreach (SkillData data in savedSkillDatas)
            {
                savedDict[data.skillID] = data;
            }
        }

        foreach (SkillConfig config in _skillConfigs)
        {
            if (config == null)
                continue;

            if (_skills.ContainsKey(config.skillID))
                continue;

            SkillData data;

            // 저장된 데이터가 있으면 사용
            if (savedDict.TryGetValue(config.skillID, out SkillData savedData))
            {
                data = savedData;
            }
            else
            {
                // 신규 스킬은 기본 데이터 생성
                data = new SkillData
                {
                    skillID = config.skillID,
                    skillLevel = 1,
                    isUnlocked = false
                };
            }

            SkillRuntime runtime = new SkillRuntime(config, data);

            _skills.Add(config.skillID, runtime);
        }
    }

    private void Update()
    {
        foreach (SkillRuntime skill in _skills.Values)
        {
            skill.Tick(Time.deltaTime);
        }
    }

    // 스킬 사용 메서드
    public void UseSkill(int skillID)
    {
        if (!_skills.TryGetValue(skillID, out SkillRuntime skill))
            return;
        
        if (skill.Config.skillEffect == null)
            return;

        if (!skill.TryUse())
            return;

        // 실제 스킬 효과 실행
        skill.Config.skillEffect.Execute(skill);
        Debug.Log($"{skill.Config.skillName} 사용!");
    }

    // 스킬 해금 시도 메서드
    public bool TryUnlockSkill(int skillID)
    {
        SkillRuntime skill = GetSkill(skillID);

        if (skill == null)
            return false;

        if (skill.Data.isUnlocked)
            return false;

        SkillUnlockCondition condition = skill.Config.unlockCondition;

        if (!condition.CanUnlock(StageManager.Instance.CurrentStage, ScoreManager.Instance.CurrentScore))
        {
            return false;
        }

        if (!ScoreManager.Instance.TrySubtractScore(condition.requiredGold))
        return false;

        skill.Data.isUnlocked = true;

        return true;
    }

    // 스킬데이터 리스트 얻는 메서드
   public List<SkillData> GetSaveSkillDatas()
    {
        List<SkillData> result = new();

        foreach (SkillRuntime skill in _skills.Values)
        {
            SkillData data = new SkillData
            {
                skillID = skill.Data.skillID,
                skillLevel = skill.Data.skillLevel,
                isUnlocked = skill.Data.isUnlocked
            };

            result.Add(data);
        }

        return result;
    }

    // 스킬 런타임 상태 얻는 메서드
    public SkillRuntime GetSkill(int skillID)
    {
        if (_skills.TryGetValue(skillID, out SkillRuntime skill))
        {
            return skill;
        }

        Debug.LogWarning($"스킬 ID {skillID}를 찾을 수 없습니다.");
        return null;
    }
}