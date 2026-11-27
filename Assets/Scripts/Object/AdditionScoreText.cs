using TMPro;
using UnityEngine;
using DG.Tweening;

public class AdditionScoreText : MonoBehaviour, IPoolable
{
    [SerializeField] private TextMeshProUGUI _text;

    private Sequence _sequence;

    /// <summary>
    /// 전투 영역에 골드 획득량을 금색으로 표시하고 사라지면 풀에 반환한다.
    /// </summary>
    public void SetText(double amount)
    {
        _sequence?.Kill();

        CanvasGroup cg = _text.GetComponent<CanvasGroup>();
        RectTransform rt = _text.rectTransform;

        cg.alpha = 1f;

        _text.text = $"+{NumberFormatter.Format(amount)}";
        _text.color = new Color(1f, 0.8f, 0.15f);

        rt.rotation = Quaternion.identity;
        rt.localScale = Vector3.one * 0.4f;

        Vector2 startPos = new Vector2(Random.Range(-170f, -65f), -65f);
        rt.anchoredPosition = startPos;

        _sequence = DOTween.Sequence();

        // 1. 등장 순간 크게 팝업
        _sequence.Append(rt.DOScale(1.2f, 0.15f).SetEase(Ease.OutBack));

        // 2. 원래 크기로 살짝 축소
        _sequence.Append(rt.DOScale(1f, 0.1f).SetEase(Ease.OutQuad));

        // 3. 위로 떠오르면서 사라짐
        _sequence.Append(rt.DOAnchorPosY(startPos.y + 105f, 0.55f).SetEase(Ease.OutQuad));
        _sequence.Join(cg.DOFade(0f, 0.55f));

        _sequence.OnComplete(() => PoolManager.Instance.Return<AdditionScoreText>(this));
    }

    public void OnSpawnFromPool()
    {
        gameObject.SetActive(true);
    }

    public void OnReturnToPool()
    {
        _sequence?.Kill();
        gameObject.SetActive(false);
    }
}
