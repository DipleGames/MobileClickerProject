using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class HUDManager : SingleTon<HUDManager>
{
    [SerializeField] private RectTransform _contentUIContainer;

    [FormerlySerializedAs("_score_text")]
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI _scoreText;
    [FormerlySerializedAs("_stage_text")]
    [SerializeField] private TextMeshProUGUI _stageText;
    [FormerlySerializedAs("_boss_timer")]
    [SerializeField] private GameObject _bossTimer;
    public TextMeshProUGUI boss_timer_text;

    /// <summary>
    /// 현재 골드를 포맷하여 HUD에 표시한다.
    /// </summary>
    public void UpdateScoreUI(double currentScore)
    {
        _scoreText.text = $"{NumberFormatter.Format(currentScore)}";
    }

    /// <summary>
    /// 스테이지 종류에 맞춰 이름, 색상, 보스 타이머를 갱신한다.
    /// </summary>
    public void UpdateStageUI(int currentStage)
    {
        if (StageManager.Instance.CurrentMonster.currentMonsterData.isBoss)
        {
            _stageText.text = $"Boss {currentStage}";
            _stageText.color = Color.red;
            _bossTimer.SetActive(true);
        }
        else
        {
            _stageText.text = $"STAGE {currentStage}";
            _stageText.color = Color.yellow;
            _bossTimer.SetActive(false);
        }
    }

    /// <summary>
    /// 버튼입력에 따라 컨텐츠 패널을 보여주는 메서드 
    /// </summary>
    public void ShowContentPanelUI(int index)
    {
        for (int i = 0; i < _contentUIContainer.childCount; i++)
        {
            bool active = i == index ? true : false;
            if (_contentUIContainer.GetChild(i).gameObject.activeSelf) // 만약 이미 켜져있는 패널인데 또 메뉴버튼입력이 들어오면 꺼버린다.
            {
                _contentUIContainer.GetChild(i).gameObject.SetActive(false);
                continue;
            }
            _contentUIContainer.GetChild(i).gameObject.SetActive(active);
        }
    }
}