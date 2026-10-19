using UnityEngine;
using System;
public class UpgradeManager : SingleTon<UpgradeManager>
{
    public double AttackPowerUpgradeCost
    {
        get
        {
            AttackConfig config = AttackManager.Instance.AttackConfig;
            double cost = config.baseUpgradeCost *  Math.Pow(config.upgradeCostMultiplier, AttackManager.Instance.AttackLevel - 1);
            return Math.Ceiling(cost);
        }
    }

    public double AbilityPowerUpgradeCost
    {
        get
        {
            AbilityConfig config = AbilityManager.Instance.AbilityConfig;
            double cost = config.baseUpgradeCost *  Math.Pow(config.upgradeCostMultiplier, AbilityManager.Instance.AbilityLevel - 1);
            return Math.Ceiling(cost);
        }
    }

    public void OnClickedAttackLevelUpgradeBtn()
    {
        double cost = AttackPowerUpgradeCost;
        // 골드 부족 시 업그레이드 중단
        if (!ScoreManager.Instance.TrySubtractScore(cost))
            return;

        AttackManager.Instance.UpgradeAttack();
    }

    public void OnClickedAbilityLevelUpgradeBtn()
    {
        double cost = AbilityPowerUpgradeCost;
        // 골드 부족 시 업그레이드 중단
        if (!ScoreManager.Instance.TrySubtractScore(cost))
            return;

        AbilityManager.Instance.UpgradeAbility();
    }
}

