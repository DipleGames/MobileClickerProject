using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;

public class HUDManager : SingleTon<HUDManager>
{
    [Header("상단 패널")]
    [SerializeField] private RectTransform _top_Paenl;

    [Header("하단 패널")]
    [SerializeField] private RectTransform _bottom_Panel;
    [SerializeField] private RectTransform _contentUIContainer;
    [SerializeField] private RectTransform _skillExcutorContainer;
    [SerializeField] private RectTransform _menuUIContainer;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI _score_text;
    [SerializeField] private TextMeshProUGUI _stage_text;
    [SerializeField] private GameObject _boss_timer;
    public TextMeshProUGUI boss_timer_text;

    protected override void Awake()
    {
        base.Awake();

        SetBottomUILayout();
    }

    private void SetBottomUILayout()
    {
        _top_Paenl.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Screen.height * 0.55f);

        _bottom_Panel.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Screen.height * 0.45f);

        const float unit = 1f / 12f;

        // 중간 (m - n)/12 영역 (n/12 ~ m/12)
         SetRectTransform(
            _contentUIContainer,
            new Vector2(0f, unit * 3f),
            new Vector2(1f, unit * 11f)
        );

        // 중간 (m - n)/12 영역 (n/12 ~ m/12)
         SetRectTransform(
            _skillExcutorContainer,
            new Vector2(0f, unit * 3.5f),
            new Vector2(1f, unit * 11f)
        );

        // 하단 영역 (0 ~ n/12)
        SetRectTransform(
            _menuUIContainer,
            new Vector2(0f, 0f),
            new Vector2(1f, unit * 3f)
        );
    }

    private void SetRectTransform(RectTransform rectTransform, Vector2 anchorMin,Vector2 anchorMax)
    {
        if (rectTransform == null)
        {
            Debug.LogError("RectTransform이 연결되지 않았습니다.");
            return;
        }

        rectTransform.anchorMin = anchorMin;
        rectTransform.anchorMax = anchorMax;

        // Anchor 영역에 정확히 맞도록 여백 제거
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;
    }

    public void UpdateScoreUI(double currentScore)
    {
        _score_text.text = $"{NumberFormatter.Format(currentScore)}";
    }

    public void UpdateStageUI(int currentStage)
    {
        if(StageManager.Instance.CurrentMonster.currentMonsterData.isBoss)
        {
            _stage_text.text = $"Boss {currentStage}";
            _stage_text.color = Color.red;
            _boss_timer.SetActive(true);
        }
        else
        {
            _stage_text.text = $"STAGE {currentStage}";
            _stage_text.color = Color.yellow;
            _boss_timer.SetActive(false);
        }
    }

    /// <summary>
    /// 버튼입력에 따라 컨텐츠 패널을 보여주는 메서드 
    /// </summary>
    public void ShowContentPanelUI(int index)
    {
        for(int i=0; i<_contentUIContainer.childCount; i++)
        {
            bool active = i == index ? true : false;
            if(_contentUIContainer.GetChild(i).gameObject.activeSelf) // 만약 이미 켜져있는 패널인데 또 메뉴버튼입력이 들어오면 꺼버린다.
            {
                _contentUIContainer.GetChild(i).gameObject.SetActive(false);
                continue;
            }
            _contentUIContainer.GetChild(i).gameObject.SetActive(active);
        }
    }
}