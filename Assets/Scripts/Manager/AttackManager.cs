using UnityEngine;
using System;

public class AttackManager : SingleTon<AttackManager>
{
    [SerializeField] private AttackConfig _config;
    [SerializeField] private double _currentAttackPower;

    public int AttackLevel { get; private set; } = 1;
    public AttackConfig AttackConfig => _config;

    private double weaponMultiplier = 1.0;
    private double rebirthMultiplier = 1.0;

    protected override void Awake()
    {
        base.Awake();
        OnAttackChanged += ViewManager.Instance.menuView.UpdateAttackLevelUpgradeUI;
    }

    public double CurrentAttackPower
    {
        get
        {
            double upgradePower = _config.baseAttack * Math.Pow(_config.attackMultiplier, AttackLevel - 1);
            _currentAttackPower = upgradePower * weaponMultiplier * rebirthMultiplier;
            return _currentAttackPower;
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