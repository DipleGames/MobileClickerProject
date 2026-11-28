using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class RageClickView : MonoBehaviour
{
    [SerializeField] private Slider _gaugeSlider; // Top_Panel에 미리 배치한 세로 분노 게이지
    [SerializeField] private Image _fillImage; // 게이지 비율에 따라 색상이 변하는 막대 이미지
    [SerializeField] private Image _icon; // 최대 분노 상태를 강조하는 불꽃 이미지
    [SerializeField] private TextMeshProUGUI _multiplierText; // 현재 피해 배율 표시

    private RageClickManager _manager; // 표시할 분노 값을 제공하는 매니저
    private int _lastMultiplier = -1; // 이전 표시 배율을 저장해 불필요한 텍스트 갱신 방지

    /// <summary>
    /// 씬에 연결한 UI에 터치와 자연 감소를 반영한 게이지와 피해 배율을 표시한다.
    /// </summary>
    private void LateUpdate()
    {
        if (_manager == null)
        {
            _manager = RageClickManager.Instance;
        }

        float gauge = _manager != null ? Mathf.Clamp01(_manager.RageGauge) : 0f;
        _gaugeSlider.SetValueWithoutNotify(gauge);
        _fillImage.color = Color.Lerp(new Color32(255, 165, 63, 255), new Color32(255, 55, 40, 255), gauge);
        bool isFull = gauge >= 0.999f;
        _icon.color = isFull ? Color.Lerp(Color.white, new Color(1f, 0.65f, 0.4f), Mathf.PingPong(Time.unscaledTime * 2f, 1f)) : Color.white;
        int multiplier = Mathf.RoundToInt((_manager != null ? _manager.RageGaugeMultiplier : 1f) * 100f);
        if (multiplier != _lastMultiplier)
        {
            _multiplierText.SetText("x{0:2}", multiplier / 100f);
            _lastMultiplier = multiplier;
        }
    }
}
