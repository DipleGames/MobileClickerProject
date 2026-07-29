using UnityEngine;

public class ScoreManager : SingleTon<ScoreManager>
{
    [SerializeField] private int _currentScore = 0;
    public int CurrentScore
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

    [SerializeField] private int _bestScore = 0;
    public int BestScore
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
}
