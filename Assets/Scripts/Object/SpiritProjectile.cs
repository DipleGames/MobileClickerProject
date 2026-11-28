using UnityEngine;

public class SpiritProjectile : MonoBehaviour, IPoolable
{
    [SerializeField, Min(1f)] private float _speed = 1800f; // 정령 부모 좌표계에서의 이동 속도다.
    [SerializeField, Min(0f)] private float _arrivalDistance = 4f; // 목표에 도착했다고 판정하는 거리다.

    private RectTransform _rect; // 투사체의 UI 위치와 회전을 조절한다.
    private RectTransform _parentRect; // 발사 위치와 목표를 표현하는 정령의 좌표계다.
    private Monster _target; // 발사 시점의 공격 대상이다.
    private MonsterData _targetData; // 같은 몬스터 오브젝트가 재사용될 때 대상을 구분한다.
    private double _damage; // 발사 시점에 계산한 피해량이다.

    /// <summary>
    /// 투사체와 정령 부모의 UI 좌표계를 저장한다.
    /// </summary>
    private void Awake()
    {
        _rect = GetComponent<RectTransform>();
        _parentRect = transform.parent as RectTransform;
    }

    /// <summary>
    /// 투사체를 정령 위치에서 발사하고 대상과 피해량을 기록한다.
    /// </summary>
    public void Launch(Monster target, double damage)
    {
        _target = target;
        _targetData = target.currentMonsterData;
        _damage = damage;
        _rect.anchoredPosition = Vector2.zero;
        _rect.localRotation = Quaternion.identity;
    }

    /// <summary>
    /// 목표로 이동하고 도착하면 피해를 준 뒤 풀에 반환한다.
    /// </summary>
    private void Update()
    {
        if (_target == null || _target.IsDie || _target.currentMonsterData != _targetData || _parentRect == null)
        {
            PoolManager.Instance.Return(this);
            return;
        }

        RectTransform targetRect = _target.transform as RectTransform;
        if (targetRect == null)
        {
            PoolManager.Instance.Return(this);
            return;
        }

        Vector3 targetWorld = targetRect.TransformPoint(targetRect.rect.center);
        Vector3 targetLocal = _parentRect.InverseTransformPoint(targetWorld);
        Vector2 current = _rect.localPosition;
        Vector2 destination = targetLocal;
        Vector2 direction = destination - current;
        if (direction.sqrMagnitude > 0.001f)
        {
            _rect.localRotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
        }

        Vector2 next = Vector2.MoveTowards(current, destination, _speed * Time.deltaTime);
        _rect.localPosition = new Vector3(next.x, next.y, _rect.localPosition.z);
        if ((destination - next).sqrMagnitude <= _arrivalDistance * _arrivalDistance)
        {
            Monster target = _target;
            double damage = _damage;
            PoolManager.Instance.Return(this);
            target.TakeDamage(false, damage);
        }
    }

    /// <summary>
    /// 풀에서 꺼낸 투사체를 활성화한다.
    /// </summary>
    public void OnSpawnFromPool()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// 공격 대상을 초기화하고 투사체를 비활성화한다.
    /// </summary>
    public void OnReturnToPool()
    {
        _target = null;
        _targetData = null;
        gameObject.SetActive(false);
    }
}
