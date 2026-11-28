using UnityEngine;

public class BasicAttackSlash : MonoBehaviour, IPoolable
{

    [SerializeField] private float _speed = 1800f; // 검기 이동 속도
    [SerializeField] private Vector2 _targetPoint = new Vector2(0.5f, 0.55f); // 목표 UI 내부의 비율 좌표
    [SerializeField] private float _arrivalDistance = 8f; // 도착으로 판정할 거리
    [SerializeField] private float _angleOffset; // 스프라이트 기본 방향 보정 각도
    [SerializeField] private float _targetYOffsetRange = 0.05f; // 목표 높이의 랜덤 범위


    private RectTransform _parentRect; // 검기의 부모 좌표계
    private Canvas _canvas; // 검기가 속한 최상위 캔버스
    private RectTransform _targetRT; // 현재 공격 대상
    private RectTransform _rect; // 검기의 RectTransform
    private Vector2 _startPosition; // 프리팹에 설정된 발사 위치
    private float _targetYOffset; // 이번 검기의 높이 오프셋


    /// <summary>
    /// 검기와 부모의 좌표 정보를 저장하고 발사 위치를 기록한다.
    /// </summary>
    private void Awake()
    {
        _rect = GetComponent<RectTransform>();
        _parentRect = _rect.parent as RectTransform;
        _startPosition = _rect.anchoredPosition;
        Canvas canvas = GetComponentInParent<Canvas>();
        if (canvas != null)
        {
            _canvas = canvas.rootCanvas;
        }
    }

    /// <summary>
    /// 검기를 목표 방향으로 회전하고 이동시킨 뒤 도착하면 풀에 반환한다.
    /// </summary>
    private void Update()
    {
        if (_targetRT == null || _rect == null || _parentRect == null || _canvas == null)
        {
            return;
        }

        Rect targetBounds = _targetRT.rect;
        Vector3 targetWorld = _targetRT.TransformPoint(new Vector3(
            Mathf.Lerp(targetBounds.xMin, targetBounds.xMax, _targetPoint.x),
            Mathf.Lerp(targetBounds.yMin, targetBounds.yMax, _targetPoint.y + _targetYOffset),
            0f));
        Camera eventCamera = _canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : _canvas.worldCamera;
        Vector2 targetScreen = RectTransformUtility.WorldToScreenPoint(eventCamera, targetWorld);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_parentRect, targetScreen, eventCamera, out Vector2 targetPosition);

        Vector2 currentPosition = _rect.localPosition;
        Vector2 direction = targetPosition - currentPosition;
        if (direction.sqrMagnitude > 0.001f)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            _rect.localRotation = Quaternion.Euler(0f, 0f, angle + _angleOffset);
        }

        Vector2 nextPosition = Vector2.MoveTowards(currentPosition, targetPosition, _speed * Time.deltaTime);
        _rect.localPosition = new Vector3(nextPosition.x, nextPosition.y, _rect.localPosition.z);

        if ((targetPosition - nextPosition).sqrMagnitude <= _arrivalDistance * _arrivalDistance)
        {
            BasicAttackImpact basicAttackImpact = PoolManager.Instance.Get<BasicAttackImpact>();
            basicAttackImpact.InitImpact(_rect.position);
            PoolManager.Instance.Return(this);
        }
    }

    /// <summary>
    /// 검기를 발사 위치로 되돌리고 이번 공격의 목표를 설정한다.
    /// </summary>
    public void InitSlash(RectTransform targetRT)
    {
        _rect.anchoredPosition = _startPosition;
        _targetRT = targetRT;
        _targetYOffset = Random.Range(-_targetYOffsetRange, _targetYOffsetRange);
    }


    /// <summary>
    /// 풀에서 꺼낸 검기를 활성화한다.
    /// </summary>
    public void OnSpawnFromPool()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// 목표를 초기화하고 풀에 반환할 검기를 비활성화한다.
    /// </summary>
    public void OnReturnToPool()
    {
        _targetRT = null;
        gameObject.SetActive(false);
    }
}
