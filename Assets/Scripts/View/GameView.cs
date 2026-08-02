using TMPro;
using UnityEngine;
using DG.Tweening;

public class GameView : MonoBehaviour
{
    [SerializeField] private RectTransform tempRT;

    public void ShowAdditionScoreText(long amount)
    {
        AdditionScoreText text = PoolManager.Instance.Get<AdditionScoreText>();
        text.SetText(amount, tempRT);
    }

}
