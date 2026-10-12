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
            if(_currentScore > _bestScore)
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

    public bool TrySubtractScore(double amount)
    {
        if(CurrentScore - amount < 0)
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
