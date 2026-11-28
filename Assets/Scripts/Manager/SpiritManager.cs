using UnityEngine;
using System;
using Unity.VisualScripting;

public class SpiritManager : SingleTon<SpiritManager>
{
    [Header("정령의 스펙")]
    [SerializeField, Min(0f)] private double _baseSpiritAttackPower = 10f; // 정령의 기본 공격력이다.
    [SerializeField, Min(0.1f)] private float _baseSpiritAttackInterval = 3f; // 정령의 기본 공격속도다.

    [Header("정령 강화 비용")]
    [SerializeField, Min(0f)] private double _baseAttackPowerUpgradeCost = 100d; // 정령 공격력의 첫 강화 비용이다.
    [SerializeField, Min(0f)] private double _baseAttackIntervalUpgradeCost = 100d; // 정령 공격 간격의 첫 강화 비용이다.
    private const double _upgradeCostMultiplier = 1.2d; // 강화할 때마다 비용에 적용하는 배율이다.
    private const int _maxSpiritAttackIntervalLevel = 101; // 최초 1레벨부터 100회 강화한 공격 간격 최대 레벨이다.

    [Header("정령의 현재 스펙")]
    [SerializeField] private double _currentSpritAttackPower;
    [SerializeField] private float _currentSpritAttackInterval;


    public int SpiritAttackPowerLevel { get; private set; } = 1;
    public int SpiritAttackIntervalLevel { get; private set; } = 1;
    public bool IsSpiritAttackIntervalMax => SpiritAttackIntervalLevel >= _maxSpiritAttackIntervalLevel;

    public event Action OnSpiritAttackPowerChange;
    public event Action OnSpiritAttackIntervalChange;

    public double SpiritAttackPowerUpgradeCost => Math.Ceiling(_baseAttackPowerUpgradeCost * Math.Pow(_upgradeCostMultiplier, SpiritAttackPowerLevel - 1));
    public double SpiritAttackIntervalUpgradeCost => Math.Ceiling(_baseAttackIntervalUpgradeCost * Math.Pow(_upgradeCostMultiplier, SpiritAttackIntervalLevel - 1));

    public double CurrentSpiritAttackPower
    {
        get
        {
            double upgradePower = _baseSpiritAttackPower * Math.Pow(1.05f, SpiritAttackPowerLevel - 1);
            _currentSpritAttackPower = upgradePower;
            return _currentSpritAttackPower;
        }

    }
    private const float _minimumAttackInterval = 0.1f; // 정령의 최소 공격 간격이다.
    public float CurrentSpiritAttackInterval
    {
        get
        {
            double interval = _baseSpiritAttackInterval * Math.Pow(0.98f, SpiritAttackIntervalLevel - 1);
            _currentSpritAttackInterval = Mathf.Max(_minimumAttackInterval, (float)interval);
            return _currentSpritAttackInterval;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        OnSpiritAttackPowerChange += ViewManager.Instance.menuView.UpdateSpiritAttackPowerLevelUpgradeUI;
        OnSpiritAttackIntervalChange += ViewManager.Instance.menuView.UpdateSpiritAttackIntervalLevelUpgradeUI;
    }

    /// <summary>
    /// 저장된 정령 강화 레벨을 적용하고 능력치와 비용 UI를 갱신한다.
    /// </summary>
    public void SetSpiritUpgradeLevel(int attackPowerLevel, int attackIntervalLevel)
    {
        SpiritAttackPowerLevel = Math.Max(1, attackPowerLevel);
        SpiritAttackIntervalLevel = Math.Max(1, attackIntervalLevel);
        OnSpiritAttackPowerChange?.Invoke();
        OnSpiritAttackIntervalChange?.Invoke();
    }

    /// <summary>
    /// 골드가 충분하면 비용을 차감하고 정령 공격력을 강화한다.
    /// </summary>
    public void UpgradeSpiritAttackPower()
    {
        if (!ScoreManager.Instance.TrySubtractScore(SpiritAttackPowerUpgradeCost))
        {
            return;
        }

        SpiritAttackPowerLevel++;
        OnSpiritAttackPowerChange?.Invoke();
    }

    /// <summary>
    /// 최대 레벨에 도달하지 않았고 골드가 충분하면 정령 공격 간격을 강화한다.
    /// </summary>
    public void UpgradeSpiritAttackInterval()
    {
        if (IsSpiritAttackIntervalMax || !ScoreManager.Instance.TrySubtractScore(SpiritAttackIntervalUpgradeCost))
        {
            return;
        }

        SpiritAttackIntervalLevel++;
        OnSpiritAttackIntervalChange?.Invoke();
    }
}
