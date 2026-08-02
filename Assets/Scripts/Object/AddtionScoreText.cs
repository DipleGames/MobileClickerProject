using TMPro;
using UnityEngine;
using DG.Tweening;

public class AdditionScoreText : MonoBehaviour, IPoolable
{
    [SerializeField] private TextMeshProUGUI _text;
    public void SetText(long amount, RectTransform tempRT)
    {
        _text.text = $"+{amount}";

        RectTransform rt = _text.rectTransform;
        rt.anchoredPosition = tempRT.anchoredPosition;   // tempRT 위치에서 시작

        CanvasGroup cg = _text.GetComponent<CanvasGroup>();
        Sequence seq = DOTween.Sequence();
        seq.Append(rt.DOAnchorPos(Vector2.up * 200f, 1.5f));
        seq.Join(cg.DOFade(0f, 2f));
        seq.OnComplete(() => Destroy(_text.gameObject));
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