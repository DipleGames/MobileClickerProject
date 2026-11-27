using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class GameView : MonoBehaviour
{
    [FormerlySerializedAs("topPanelRT")]
    [SerializeField] private RectTransform _topPanelRT;

    [SerializeField] private RectTransform _healthBar;

    /// <summary>
    /// 몬스터 주변에 피해량을 띄우고 체력바 하단을 이동 한계로 전달한다.
    /// </summary>
    public void ShowDamageText(bool isSkill, double amount)
    {
        DamageText text = PoolManager.Instance.Get<DamageText>();
        Vector3 healthBarBottom = _healthBar.TransformPoint(new Vector3(0f, _healthBar.rect.yMin));
        text.SetText(isSkill, amount, healthBarBottom);
    }

    /// <summary>
    /// 전투 영역에 골드 획득량을 표시한다.
    /// </summary>
    public void ShowAdditionScoreText(double amount)
    {
        AdditionScoreText text = PoolManager.Instance.Get<AdditionScoreText>();
        text.SetText(amount);
    }

    /// <summary>
    /// 터치 연출 메서드
    /// </summary>
    public void ShowTouchEffect()
    {
        //topPanelRT.DOShakeAnchorPos(0.15f, 15f, 20, 90);
    }

}
