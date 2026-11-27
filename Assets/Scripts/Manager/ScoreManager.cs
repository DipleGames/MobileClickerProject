using JetBrains.Annotations;
using UnityEngine;

public class ScoreManager : SingleTon<ScoreManager>
{
    [SerializeField] private double _currentScore = 0;
    public double CurrentScore
    {
        get
        {
            return _currentScore;
        }
        set
        {
            _currentScore = value;
            HUDManager.Instance.UpdateScoreUI(_currentScore);
            if (_currentScore > _bestScore)
            {
                _bestScore = _currentScore;
            }
        }
    }

    [SerializeField] private double _bestScore = 0;
    public double BestScore
    {
        get
        {
            return _bestScore;   
        }
        set
        {
            _bestScore = value;
        }
    }

    public void AddScore(double amount)
    {
        CurrentScore += amount;
    }

    /// <summary>
    /// 골드가 충분하면 지정한 양을 차감하고 성공 여부를 반환한다.
    /// </summary>
    public bool TrySubtractScore(double amount)
    {
        if (CurrentScore - amount < 0)
        {
            return false;
        }
        
        CurrentScore -= amount;
        return true;
    }

    public void GiveOfflineReward(SaveData saveData, long reward)
    {
        saveData.currentScore += reward;
    }
}
