using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering;

public class AdditionScoreText : MonoBehaviour, IPoolable
{
    [SerializeField] private TextMeshProUGUI _text;
    public void SetText(bool isSkill, double amount, RectTransform tempRT)
    {
        CanvasGroup cg = _text.GetComponent<CanvasGroup>();
        cg.alpha = 1f;

        // 스킬 공격이면 파란색, 일반 공격이면 흰색
        _text.color = isSkill ? Color.cyan : Color.white;

        _text.text = $"+{NumberFormatter.Format(amount)}";

        RectTransform rt = _text.rectTransform;

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