using System;
using UnityEngine;

[Serializable]
public class SkillUnlockCondition
{
    public int requiredStage;       // 필요 스테이지
    public double requiredGold;     // 필요 골드

    public bool CanUnlock(int currentStage, double currentGold)
    {
        return currentStage >= requiredStage && currentGold >= requiredGold;
    }
}