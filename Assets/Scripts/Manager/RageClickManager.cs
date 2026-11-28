using UnityEngine;

public class RageClickManager : SingleTon<RageClickManager>
{
    [Header("게이지")]
    [SerializeField, Min(0f)] private float _rageGauge; // 현재 분노 게이지 (0~1)
    [SerializeField, Min(0.01f)] private float _fastClickInterval = 0.25f; // 빠른 터치로 판정할 최대 간격 (초)
    [SerializeField, Min(0f)] private float _gaugeIncreasePerFastClick = 0.08f; // 빠른 터치 1회당 게이지 증가량
    [SerializeField, Min(0f)] private float _gaugeDecreasePerSlowClick = 0.05f; // 느린 터치 1회당 게이지 감소량
    [SerializeField, Min(0f)] private float _gaugeDecayPerSecond = 0.1f; // 입력이 없을 때 초당 게이지 감소량

    private float _lastClickTime; // 직전 터치가 발생한 게임 시간
    private bool _hasClicked; // 첫 터치 이후인지 여부

    /// <summary>
    /// 현재 분노 게이지를 0~1 범위로 반환한다.
    /// </summary>
    public float RageGauge => _rageGauge;

    /// <summary>
    /// 현재 분노 게이지를 1~3배 피해 계수로 변환해 반환한다.
    /// </summary>
    public float RageGaugeMultiplier => Mathf.Lerp(1f, 3f, _rageGauge);

    /// <summary>
    /// 빠른 터치에는 게이지를 더하고 느린 터치에는 게이지를 낮춘다.
    /// </summary>
    public void OnClick()
    {
        float now = Time.time;

        if (_hasClicked)
        {
            float elapsed = now - _lastClickTime;
            if (elapsed <= _fastClickInterval)
            {
                _rageGauge += _gaugeIncreasePerFastClick;
            }
            else
            {
                _rageGauge -= _gaugeDecreasePerSlowClick;
            }
        }

        _lastClickTime = now;
        _hasClicked = true;
        ClampRageGauge();
    }

    /// <summary>
    /// 입력이 없는 동안 경과 시간에 비례해 게이지를 감소시킨다.
    /// </summary>
    private void Update()
    {
        if (_rageGauge <= 0f)
        {
            return;
        }

        _rageGauge -= _gaugeDecayPerSecond * Time.deltaTime;
        ClampRageGauge();
    }

    /// <summary>
    /// 분노 게이지를 0~1 범위로 제한한다.
    /// </summary>
    private void ClampRageGauge()
    {
        _rageGauge = Mathf.Clamp01(_rageGauge);
    }
}
