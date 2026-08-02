using UnityEngine;

public class ScoreManager : SingleTon<ScoreManager>
{
    [SerializeField] private long _currentScore = 0;
    public long CurrentScore
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

    [SerializeField] private long _bestScore = 0;
    public long BestScore
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

    public void GiveOfflineReward(SaveData saveData, long reward)
    {
        saveData.currentScore += reward;
    }
}
