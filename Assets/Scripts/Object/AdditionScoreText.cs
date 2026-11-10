using TMPro;
using UnityEngine;
using DG.Tweening;

public class AdditionScoreText : MonoBehaviour, IPoolable
{
    [SerializeField] private TextMeshProUGUI _text;

    private Sequence _sequence;

    public void SetText(double amount, RectTransform tempRT)
    {
        _sequence?.Kill();

        CanvasGroup cg = _text.GetComponent<CanvasGroup>();
        RectTransform rt = _text.rectTransform;

        cg.alpha = 1f;

        _text.text = $"+{NumberFormatter.Format(amount)}";
        _text.color = new Color(1f, 0.8f, 0.15f);

        rt.rotation = Quaternion.identity;
        rt.localScale = Vector3.one * 0.4f;

        float randomX = Random.Range(-60f, 60f);

        Vector2 startPos = tempRT.anchoredPosition + new Vector2(randomX, -150f);
        rt.anchoredPosition = startPos;

        _sequence = DOTween.Sequence();

        // 1. 등장 순간 크게 팝업
        _sequence.Append(rt.DOScale(1.4f, 0.2f).SetEase(Ease.OutBack));

        // 2. 원래 크기로 살짝 축소
        _sequence.Append(rt.DOScale(1f, 0.15f).SetEase(Ease.OutQuad));

        // 3. 위로 떠오르면서 사라짐
        _sequence.Append(rt.DOAnchorPosY(startPos.y + 180f, 0.8f).SetEase(Ease.OutQuad));
        _sequence.Join(cg.DOFade(0f, 0.8f));

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