using UnityEngine;
using System.Collections.Generic;

public class SaveData
{
    public int currentStage; // 현재 스테이지
    public double currentScore; // 현재 스코어
    public int currentAttackLevel; // 현재 공격력
    public int currentAbilityLevel; // 현재 주문력
    public string lastSaveTime; // 마지막 끌떄 시간

    // 스킬 성장 데이터
    public List<SkillData> skillDatas = new();

    // 스킬 장착 정보 (슬롯 6개)
    public int[] equippedSkillIDs = { -1, -1, -1, -1, -1, -1 };

}
