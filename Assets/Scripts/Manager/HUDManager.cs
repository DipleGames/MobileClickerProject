using TMPro;
using UnityEngine;

public class HUDManager : SingleTon<HUDManager>
{
    [Header("ScoreUI")]
    public TextMeshProUGUI score_Text;

    public void UpdateScoreUI(int currentScore)
    {
        score_Text.text = $"{currentScore}";
    }
}
