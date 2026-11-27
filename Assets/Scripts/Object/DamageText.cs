using TMPro;
using UnityEngine;
using DG.Tweening;

public class DamageText : MonoBehaviour, IPoolable
{
    [SerializeField] private TextMeshProUGUI _text;
    /// <summary>
    /// 피해량을 몬스터 주변에 분산하고 체력바 아래에서 페이드아웃한다.
    /// </summary>
    public void SetText(bool isSkill, double amount, Vector3 healthBarBottom)
    {
        CanvasGroup cg = _text.GetComponent<CanvasGroup>();
        cg.alpha = 1f;

        _text.color = isSkill ? new Color32(151, 217, 255, 255) : new Color32(255, 233, 200, 255);
        _text.text = NumberFormatter.Format(amount);

        RectTransform rt = _text.rectTransform;
        RectTransform parent = (RectTransform)rt.parent;

        float ceiling = parent.InverseTransformPoint(healthBarBottom).y - parent.rect.center.y - rt.rect.height * 0.6f - 16f;
        float startY = Mathf.Min(Random.Range(140f, 200f), ceiling - 45f);
        float endY = Mathf.Min(startY + Random.Range(155f, 205f), ceiling);
        float startX = Random.Range(-110f, 110f);
        float endX = startX + Random.Range(-90f, 90f);

        rt.rotation = Quaternion.identity;
        rt.anchoredPosition = new Vector2(startX, startY);
        rt.localScale = Vector3.one * (isSkill ? 1.12f : 1f);

        Sequence seq = DOTween.Sequence();
        seq.Append(rt.DOAnchorPos(new Vector2(endX, endY), 1.25f).SetEase(Ease.OutQuad));
        seq.Insert(0.3f, cg.DOFade(0f, 0.95f).SetEase(Ease.InQuad));

        seq.OnComplete(() => PoolManager.Instance.Return<DamageText>(this));
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
