using UnityEngine;
using System;


public class DropManager : SingleTon<DropManager>
{
    [SerializeField] private DropConfig _config;
    [SerializeField] private double _currentDropProb;

    public int DropLevel { get; private set; } = 1;
    public DropConfig DropConfig => _config;

    public event Action OnDropProbChanged;
    protected override void Awake()
    {
        base.Awake();
        OnDropProbChanged += ViewManager.Instance.menuView.UpdateDropLevelUpgradeUI;
    }

    public double CurrentDropProb
    {
        get
        {
            double upgradePower = _config.baseProb + _config.dropProbBonus * DropLevel;
            _currentDropProb = upgradePower;
            return _currentDropProb;
        }
    }

    public void UpgradeDropProb()
    {
        DropLevel++;
        OnDropProbChanged?.Invoke();
    }


    public void SetDropLevel(int level)
    {
        DropLevel = Math.Max(1, level);

        OnDropProbChanged?.Invoke();
    }

    public void DropScore(double damage)
    {
        bool isSuccess = UnityEngine.Random.value < CurrentDropProb / 100f;

        if (isSuccess)
        {
            ScoreManager.Instance.AddScore(damage);
            ViewManager.Instance.gameView.ShowAdditionScoreText(damage);

        }
    }
}
