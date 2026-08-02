using TMPro;
using UnityEngine;

public class HUDManager : SingleTon<HUDManager>
{
    [Header("상단 패널")]
    [SerializeField] private RectTransform _top_Paenl;

    [Header("하단 패널")]
    [SerializeField] private RectTransform _bottom_Panel;
    [SerializeField] private RectTransform _contentUIContainer;
    [SerializeField] private RectTransform _menuUIContainer;

    [Header("Score UI")]
    [SerializeField] private TextMeshProUGUI _scoreText;

    protected override void Awake()
    {
        base.Awake();

        SetBottomUILayout();
    }

    private void SetBottomUILayout()
    {
        _top_Paenl.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Screen.height * 0.5f);
        _bottom_Panel.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Screen.height * 0.5f);

        const float menuRatio = 1f / 6f;

        // 위쪽 5/6 영역
        SetRectTransform(
            _contentUIContainer,
            new Vector2(0f, menuRatio),
            new Vector2(1f, 1f)
        );

        // 아래쪽 1/6 영역
        SetRectTransform(
            _menuUIContainer,
            new Vector2(0f, 0f),
            new Vector2(1f, menuRatio)
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

    public void UpdateScoreUI(long currentScore)
    {
        _scoreText.text = currentScore.ToString();
    }
}