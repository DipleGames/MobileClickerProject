using UnityEngine;
using System;

public class AttackManager : SingleTon<AttackManager>
{
    [SerializeField] private AttackConfig config;

    public int AttackLevel { get; private set; } = 1;

    private double weaponMultiplier = 1.0;
    private double rebirthMultiplier = 1.0;

    public double CurrentAttackPower
    {
        get
        {
            double upgradePower = config.baseAttack * Math.Pow(config.attackMultiplier, AttackLevel - 1);

            return upgradePower * weaponMultiplier * rebirthMultiplier;
        }
    }

    public event Action OnAttackChanged;

    public void UpgradeAttack()
    {
        AttackLevel++;

        OnAttackChanged?.Invoke();
    }

    public void SetWeaponMultiplier(double multiplier)
    {
        weaponMultiplier = multiplier;

        OnAttackChanged?.Invoke();
    }

    public void SetRebirthMultiplier(double multiplier)
    {
        rebirthMultiplier = multiplier;

        OnAttackChanged?.Invoke();
    }

    public void SetAttackLevel(int level)
    {
        AttackLevel = Math.Max(1, level);

        OnAttackChanged?.Invoke();
    }
}