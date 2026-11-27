using UnityEngine;
using UnityEngine.Serialization;
using System;
public class UpgradeManager : SingleTon<UpgradeManager>
{
    [FormerlySerializedAs("_upgrade_Fail")]
    [Header("업그레이드 사운드 SFX")]
    [SerializeField] private AudioClip _upgradeFail;
    [FormerlySerializedAs("_upgrade_Success")]
    [SerializeField] private AudioClip _upgradeSuccess;

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

    public double DropProbUpgradeCost
    {
        get
        {
            DropConfig config = DropManager.Instance.DropConfig;
            double cost = config.baseUpgradeCost *  Math.Pow(config.upgradeCostMultiplier, DropManager.Instance.DropLevel - 1);
            return Math.Ceiling(cost);
        }
    }

    /// <summary>
    /// 골드를 지불해 공격력 레벨을 올리고 결과 사운드를 재생한다.
    /// </summary>
    public void OnClickedAttackLevelUpgradeBtn()
    {
        double cost = AttackPowerUpgradeCost;
        // 골드 부족 시 업그레이드 중단
        if (!ScoreManager.Instance.TrySubtractScore(cost))
        {
            AudioManager.Instance.PlaySFX(_upgradeFail);
            return;
        }

        AudioManager.Instance.PlaySFX(_upgradeSuccess);
        AttackManager.Instance.UpgradeAttack();
    }

    /// <summary>
    /// 골드를 지불해 능력 레벨을 올리고 결과 사운드를 재생한다.
    /// </summary>
    public void OnClickedAbilityLevelUpgradeBtn()
    {
        double cost = AbilityPowerUpgradeCost;
        // 골드 부족 시 업그레이드 중단
        if (!ScoreManager.Instance.TrySubtractScore(cost))
        {
            AudioManager.Instance.PlaySFX(_upgradeFail);
            return;
        }

        AudioManager.Instance.PlaySFX(_upgradeSuccess);
        AbilityManager.Instance.UpgradeAbility();
    }

    /// <summary>
    /// 골드를 지불해 드롭 레벨을 올리고 결과 사운드를 재생한다.
    /// </summary>
    public void OnClickedDropLevelUpgradeBtn()
    {
        double cost = DropProbUpgradeCost;
        // 골드 부족 시 업그레이드 중단
        if (!ScoreManager.Instance.TrySubtractScore(cost))
        {
            AudioManager.Instance.PlaySFX(_upgradeFail);
            return;
        }

        AudioManager.Instance.PlaySFX(_upgradeSuccess);
        DropManager.Instance.UpgradeDropProb();
    }
}

