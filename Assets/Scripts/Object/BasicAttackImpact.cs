using DG.Tweening;
using UnityEngine;

public class BasicAttackImpact : MonoBehaviour, IPoolable
{
    private RectTransform _rect; // 섬광의 크기와 위치
    private CanvasGroup _canvasGroup; // 섬광의 투명도
    private Vector3 _baseScale; // 프리팹에 설정된 기본 크기
    private Sequence _impactSequence; // 재생 중인 섬광 연출

    /// <summary>
    /// 섬광 연출에 필요한 컴포넌트와 기본 크기를 저장한다.
    /// </summary>
    private void Awake()
    {
        _rect = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();
        _baseScale = _rect.localScale;
    }

    /// <summary>
    /// 지정한 위치에 섬광을 표시하고 확대와 페이드 연출을 재생한다.
    /// </summary>
    public void InitImpact(Vector3 worldPosition)
    {
        _impactSequence?.Kill();
        _rect.position = worldPosition;
        _rect.localScale = _baseScale * 0.5f;
        _canvasGroup.alpha = 1f;

        _impactSequence = DOTween.Sequence().SetLink(gameObject);
        _impactSequence.Append(_rect.DOScale(_baseScale, 0.05f).SetEase(Ease.OutQuad));
        _impactSequence.Append(_rect.DOScale(_baseScale * 1.1f, 0.1f).SetEase(Ease.OutQuad));
        _impactSequence.Join(_canvasGroup.DOFade(0f, 0.1f));
        _impactSequence.OnComplete(() => PoolManager.Instance.Return(this));
    }

    /// <summary>
    /// 풀에서 꺼낸 섬광을 활성화한다.
    /// </summary>
    public void OnSpawnFromPool()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// 진행 중인 섬광 연출을 종료하고 오브젝트를 비활성화한다.
    /// </summary>
    public void OnReturnToPool()
    {
        _impactSequence?.Kill();
        _impactSequence = null;
        gameObject.SetActive(false);
    }
}
