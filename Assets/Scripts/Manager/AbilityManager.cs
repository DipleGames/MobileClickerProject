using UnityEngine;
using System;


public class AbilityManager : SingleTon<AbilityManager>
{
    [SerializeField] private AbilityConfig _config;
    [SerializeField] private double _currentAbilityPower;

    public int AbilityLevel { get; private set; } = 1;
    public AbilityConfig AbilityConfig => _config;

    private double weaponMultiplier = 1.0;
    private double rebirthMultiplier = 1.0;

    protected override void Awake()
    {
        base.Awake();
        OnAbilityChanged += ViewManager.Instance.menuView.UpdateAbilityLevelUpgradeUI;
    }

    public double CurrentAbilityPower
    {
        get
        {
            double upgradePower = _config.baseAbility * Math.Pow(_config.abilityMultiplier, AbilityLevel - 1);
            _currentAbilityPower = upgradePower;
            return _currentAbilityPower;
        }
    }

    public event Action OnAbilityChanged;

    public void UpgradeAbility()
    {
        AbilityLevel++;
        OnAbilityChanged?.Invoke();
    }


    public void SetRebirthMultiplier(double multiplier)
    {
        rebirthMultiplier = multiplier;

        OnAbilityChanged?.Invoke();
    }

    public void SetAbilityLevel(int level)
    {
        AbilityLevel = Math.Max(1, level);

        OnAbilityChanged?.Invoke();
    }
}
