using JetBrains.Annotations;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MenuView : MonoBehaviour
{
    [Header("공격력 업데이트 관련")]
    public TextMeshProUGUI AttackLevelUpgrade_Text;
    public TextMeshProUGUI currentAttackPower_Text;
    public TextMeshProUGUI AttackLevelUpgradeCost_Text;

    [Header("주문력 업데이트 관련")]
    public TextMeshProUGUI AbilityLevelUpgrade_Text;
    public TextMeshProUGUI currentAbilityPower_Text;
    public TextMeshProUGUI AbilityLevelUpgradeCost_Text;

    [Header("드랍확률 업데이트 관련")]
    public TextMeshProUGUI DropLevelUpgrade_Text;
    public TextMeshProUGUI currentDropProb_Text;
    public TextMeshProUGUI DropLevelUpgradeCost_Text;

    [Header("스킬 관련")]
    public GameObject skill_Panel;
    public Button skill_Btn_Prefab;

    [Header("정령 공격력 업데이트 관련")]
    public TextMeshProUGUI SpiritAttackPower_Text;
    public TextMeshProUGUI SpiritAttackPowerCost_Text; // 정령 공격력 강화 비용을 표시한다.

    [Header("정령 공격력 업데이트 관련")]
    public TextMeshProUGUI SpiritAttackInterval_Text;
    public TextMeshProUGUI SpiritAttackIntervalCost_Text; // 정령 공격 간격 강화 비용을 표시한다.


    

    // 공격력 업그레이드 시 UI변경
    public void UpdateAttackLevelUpgradeUI()
    {
        AttackLevelUpgrade_Text.text = $"공격력 강화 Lv.{AttackManager.Instance.AttackLevel}";
        currentAttackPower_Text.text = $"현재 공격력 {NumberFormatter.Format(AttackManager.Instance.CurrentAttackPower)}";
        AttackLevelUpgradeCost_Text.text = $"{NumberFormatter.Format(UpgradeManager.Instance.AttackPowerUpgradeCost)}";
    }

    // 주문력 업그레이드 시 UI변경
    public void UpdateAbilityLevelUpgradeUI()
    {
        AbilityLevelUpgrade_Text.text = $"주문력 강화 Lv.{AbilityManager.Instance.AbilityLevel}";
        currentAbilityPower_Text.text = $"현재 주문력 {NumberFormatter.Format(AbilityManager.Instance.CurrentAbilityPower)}";
        AbilityLevelUpgradeCost_Text.text = $"{NumberFormatter.Format(UpgradeManager.Instance.AbilityPowerUpgradeCost)}";
    }

    // 드랍확률 업그레이드 시 UI변경
    public void UpdateDropLevelUpgradeUI()
    {
        DropLevelUpgrade_Text.text = $"드랍확률 강화 Lv.{DropManager.Instance.DropLevel}";
        currentDropProb_Text.text = $"현재 확률 {NumberFormatter.FormatPercent(DropManager.Instance.CurrentDropProb)}";
        DropLevelUpgradeCost_Text.text = $"{NumberFormatter.Format(UpgradeManager.Instance.DropProbUpgradeCost)}";
    }

    // 스킬 버튼 세팅
    /// <summary>
    /// 스킬 설정을 읽어 패널에 스킬 버튼을 생성한다.
    /// </summary>
    public void SetSkillPanelUI()
    {
        foreach (SkillConfig config in SkillManager.Instance.SkillConfigs)
        {
            // 스킬 ID를 통해 SkillRuntime 가져오기
            SkillRuntime skill = SkillManager.Instance.GetSkill(config.skillID);

            if (skill == null)
            {
                continue;
            }

            // 버튼 생성
            Button skillBtnInstance = Instantiate(skill_Btn_Prefab, skill_Panel.transform);

            // 스킬 연결
            SkillBtn skillBtn = skillBtnInstance.GetComponent<SkillBtn>();
            skillBtn.SetSkill(skill);

            // UI 설정
            Transform btnTransform = skillBtnInstance.transform;

            btnTransform.GetChild(0).GetComponent<Image>().sprite = config.skillIcon;

            btnTransform.GetChild(1).GetComponent<TextMeshProUGUI>().text = config.skillName;

            btnTransform.GetChild(2).GetComponent<TextMeshProUGUI>().text = config.skillDesc;

            skillBtn.unlockCondition_text.text = $"스테이지 {skill.Config.unlockCondition.requiredStage}에서 잠금 해제";
            skillBtn.lockOverlay.SetActive(!skill.Data.isUnlocked);
        }
    }

    /// <summary>
    /// 정령 공격력과 다음 강화 비용을 축약 형식으로 표시한다.
    /// </summary>
    public void UpdateSpiritAttackPowerLevelUpgradeUI()
    {
        SpiritAttackPower_Text.text = NumberFormatter.Format(SpiritManager.Instance.CurrentSpiritAttackPower);
        SpiritAttackPowerCost_Text.text = NumberFormatter.Format(SpiritManager.Instance.SpiritAttackPowerUpgradeCost);
    }

    /// <summary>
    /// 정령 공격 간격과 강화 비용 또는 최대 강화 상태를 표시한다.
    /// </summary>
    public void UpdateSpiritAttackIntervalLevelUpgradeUI()
    {
        SpiritAttackInterval_Text.text = $"{SpiritManager.Instance.CurrentSpiritAttackInterval:F1}";
        bool isMax = SpiritManager.Instance.IsSpiritAttackIntervalMax;
        SpiritAttackIntervalCost_Text.text = isMax ? "MAX" : NumberFormatter.Format(SpiritManager.Instance.SpiritAttackIntervalUpgradeCost);
        Button upgradeButton = SpiritAttackIntervalCost_Text.GetComponentInParent<Button>();
        if (upgradeButton != null)
        {
            upgradeButton.interactable = !isMax;
        }
    }
}
