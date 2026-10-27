using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering;

public class AdditionScoreText : MonoBehaviour, IPoolable
{
    [SerializeField] private TextMeshProUGUI _text;
    public void SetText(double amount, RectTransform tempRT)
    {
        CanvasGroup cg = _text.GetComponent<CanvasGroup>();
        cg.alpha = 1;

        _text.text = $"+{NumberFormatter.Format(amount)}";

        RectTransform rt = _text.rectTransform;

        // 부모의 회전과 관계없이 월드 회전 0도 유지
        rt.rotation = Quaternion.identity;

        rt.anchoredPosition = tempRT.anchoredPosition + new Vector2(0, -200f);

        Sequence seq = DOTween.Sequence();

        float randomX = Random.Range(-100f, 100f);

        seq.Append(rt.DOAnchorPos(new Vector2(randomX, 200f), 1.5f).SetRelative());
        seq.Join(cg.DOFade(0f, 2f));
        seq.OnComplete(() => PoolManager.Instance.Return<AdditionScoreText>(this));
    }

    public void OnSpawnFromPool()
    {
        gameObject.SetActive(true);
    }

    public void OnReturnToPool()
    {
        gameObject.SetActive(false);
    }
}