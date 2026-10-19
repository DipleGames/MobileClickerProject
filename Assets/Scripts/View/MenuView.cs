using TMPro;
using UnityEngine;

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


    public void UpdateAttackLevelUpgradeUI()
    {
        AttackLevelUpgrade_Text.text = $"공격력 강화 Lv.{AttackManager.Instance.AttackLevel}";
        currentAttackPower_Text.text = $"현재 공격력 {NumberFormatter.Format(AttackManager.Instance.CurrentAttackPower)}";
        AttackLevelUpgradeCost_Text.text = $"{NumberFormatter.Format(UpgradeManager.Instance.AttackPowerUpgradeCost)}";
    }

    public void UpdateAbilityLevelUpgradeUI()
    {
        AbilityLevelUpgrade_Text.text = $"주문력 강화 Lv.{AbilityManager.Instance.AbilityLevel}";
        currentAbilityPower_Text.text = $"현재 주문력 {NumberFormatter.Format(AbilityManager.Instance.CurrentAbilityPower)}";
        AbilityLevelUpgradeCost_Text.text = $"{NumberFormatter.Format(UpgradeManager.Instance.AbilityPowerUpgradeCost)}";
    }
}
