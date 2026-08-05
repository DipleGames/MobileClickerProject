using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering;

public class AdditionScoreText : MonoBehaviour, IPoolable
{
    [SerializeField] private TextMeshProUGUI _text;
    public void SetText(long amount, RectTransform tempRT)
    {
        CanvasGroup cg = _text.GetComponent<CanvasGroup>();
        cg.alpha = 1;
        
        _text.text = $"+{amount}";

        RectTransform rt = _text.rectTransform;
        rt.anchoredPosition = tempRT.anchoredPosition + new Vector2(0, 250f);   // tempRT 위치에서 시작

        cg = _text.GetComponent<CanvasGroup>();
        Sequence seq = DOTween.Sequence();

        float randomX = Random.Range(-100f, 100f);
        seq.Append(rt.DOAnchorPos(rt.anchoredPosition + new Vector2(randomX, 200f), 1.5f).SetRelative());
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