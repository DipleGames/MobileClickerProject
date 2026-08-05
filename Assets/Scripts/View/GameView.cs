using TMPro;
using UnityEngine;
using DG.Tweening;

public class GameView : MonoBehaviour
{
    [SerializeField] private RectTransform tempRT; // 추가스코어 텍스트가 시작될 위치
    [SerializeField] private RectTransform topPanelRT;

    /// <summary>
    /// 추가스코어 텍스트 출력을 담당하는 메서드
    /// </summary>
    /// <param name="amount"></param>
    public void ShowAdditionScoreText(long amount)
    {
        AdditionScoreText text = PoolManager.Instance.Get<AdditionScoreText>();
        text.SetText(amount, tempRT);
    }

    /// <summary>
    /// 터치 연출 메서드
    /// </summary>
    public void ShowTouchEffect()
    {
        //topPanelRT.DOShakeAnchorPos(0.15f, 15f, 20, 90);
    }

}
