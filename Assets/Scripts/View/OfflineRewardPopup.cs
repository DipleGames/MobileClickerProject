using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OfflineRewardPopup : MonoBehaviour
{
    [SerializeField] private GameObject _panel; // 보상 팝업 전체 영역
    [SerializeField] private TextMeshProUGUI _elapsedText; // 오프라인 경과 시간 표시
    [SerializeField] private TextMeshProUGUI _rewardText; // 지급된 골드 표시
    [SerializeField] private Button _confirmButton; // 팝업을 닫는 확인 버튼

    /// <summary>
    /// 확인 버튼에 팝업 닫기 동작을 연결한다.
    /// </summary>
    private void Awake()
    {
        _confirmButton.onClick.AddListener(Close);
        _panel.SetActive(false);
    }

    /// <summary>
    /// 지급된 골드와 오프라인 시간을 표시한다.
    /// </summary>
    public void Show(long reward, TimeSpan elapsed)
    {
        long totalHours = (long)elapsed.TotalHours;
        _elapsedText.text = $"오프라인 시간  {totalHours}시간 {elapsed.Minutes}분 {elapsed.Seconds}초";
        _rewardText.text = $"+{NumberFormatter.Format(reward)} 골드";
        _panel.SetActive(true);
        _panel.transform.SetAsLastSibling();
    }

    /// <summary>
    /// 오프라인 보상 팝업을 닫는다.
    /// </summary>
    private void Close()
    {
        _panel.SetActive(false);
    }
}
