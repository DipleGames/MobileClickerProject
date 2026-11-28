using UnityEngine;
using System;

public class AttackManager : SingleTon<AttackManager>
{
    [SerializeField] private AttackConfig _config;
    [SerializeField] private double _currentAttackPower;

    public int AttackLevel { get; private set; } = 1;
    public AttackConfig AttackConfig => _config;

    private double _weaponMultiplier = 1.0;
    private double _rebirthMultiplier = 1.0;
    private double _buffMultiplier = 1.0; // 현재 공격력 버프 배율

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
            _currentAttackPower = upgradePower * _weaponMultiplier * _rebirthMultiplier * _buffMultiplier;
            return _currentAttackPower;
        }
    }

    public event Action OnAttackChanged;

    /// <summary>
    /// 공격력 버프 배율을 변경하고 공격력 UI를 갱신한다.
    /// </summary>
    public void SetBuffMultiplier(double multiplier)
    {
        _buffMultiplier = multiplier;
        OnAttackChanged?.Invoke();
    }

    public void UpgradeAttack()
    {
        AttackLevel++;
        OnAttackChanged?.Invoke();
    }

    /// <summary>
    /// 무기 배율을 설정하고 공격력 변경을 알린다.
    /// </summary>
    public void SetWeaponMultiplier(double multiplier)
    {
        _weaponMultiplier = multiplier;

        OnAttackChanged?.Invoke();
    }

    /// <summary>
    /// 환생 배율을 설정하고 공격력 변경을 알린다.
    /// </summary>
    public void SetRebirthMultiplier(double multiplier)
    {
        _rebirthMultiplier = multiplier;

        OnAttackChanged?.Invoke();
    }

    public void SetAttackLevel(int level)
    {
        AttackLevel = Math.Max(1, level);

        OnAttackChanged?.Invoke();
    }
}
