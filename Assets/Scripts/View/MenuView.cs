using TMPro;
using UnityEngine;

public class MenuView : MonoBehaviour
{
    [Header("공격력 업데이트 관련")]
    public TextMeshProUGUI AttackLevelUpgrade_Text;
    public TextMeshProUGUI currentAttackPower_Text;
    public TextMeshProUGUI AttackLevelUpgradeCost_Text;

    public void UpdateAttackLevelUpgradeUI()
    {
        AttackLevelUpgrade_Text.text = $"공격력 강화 Lv.{AttackManager.Instance.AttackLevel}";
        currentAttackPower_Text.text = $"현재 공격력 {NumberFormatter.Format(AttackManager.Instance.CurrentAttackPower)}";
        AttackLevelUpgradeCost_Text.text = $"{NumberFormatter.Format(UpgradeManager.Instance.AttackPowerUpgradeCost)}";
    }
}
