using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using DG.Tweening;

public class GameView : MonoBehaviour
{
    [FormerlySerializedAs("tempRT")]
    [SerializeField] private RectTransform _tempRT; // 추가스코어 텍스트가 시작될 위치
    [FormerlySerializedAs("topPanelRT")]
    [SerializeField] private RectTransform _topPanelRT;

    /// <summary>
    /// 데미지 텍스트 출력을 담당하는 메서드
    /// </summary>
    /// <param name="amount"></param>
    public void ShowDamageText(bool isSkill, double amount)
    {
        DamageText text = PoolManager.Instance.Get<DamageText>();
        text.SetText(isSkill, amount, _tempRT);
    }

    /// <summary>
    /// 추가스코어 텍스트 출력을 담당하는 메서드
    /// </summary>
    /// <param name="amount"></param>
    public void ShowAdditionScoreText(double amount)
    {
        AdditionScoreText text = PoolManager.Instance.Get<AdditionScoreText>();
        text.SetText(amount, _tempRT);
    }

    /// <summary>
    /// 터치 연출 메서드
    /// </summary>
    public void ShowTouchEffect()
    {
        //topPanelRT.DOShakeAnchorPos(0.15f, 15f, 20, 90);
    }

}
