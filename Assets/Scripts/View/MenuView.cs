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

    [Header("스킬 관련")]
    public GameObject skill_Panel;
    public Button skill_Btn_Prefab;

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

    // 스킬 버튼 세팅
    public void SetSkillPanelUI()
    {
        for(int i=0; i<SkillManager.Instance.SkillConfigs.Count; i++)
        {
            Button skill_Btn_Instance = Instantiate(skill_Btn_Prefab, skill_Panel.transform);
            SkillExecutor skillExecutor = skill_Btn_Instance.GetComponent<SkillExecutor>();
            skillExecutor.SetSkill(SkillManager.Instance.SkillConfigs[i].skillID);
            skill_Btn_Instance.transform.GetChild(0).GetComponent<Image>().sprite = SkillManager.Instance.SkillConfigs[i].skillIcon;
            skill_Btn_Instance.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = SkillManager.Instance.SkillConfigs[i].skillName;
            skill_Btn_Instance.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = SkillManager.Instance.SkillConfigs[i].skillDesc;
        }
    }
}
