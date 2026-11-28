using UnityEngine;
using UnityEngine.Rendering;

public class Spirit : MonoBehaviour
{
    [SerializeField] private Animator _animator; // 정령의 공격 애니메이션을 재생한다.


    private float _attackTimer; // 다음 공격까지 경과한 시간이다.
    private static readonly int _attackHash = Animator.StringToHash("Spirit_Attack"); // 공격 애니메이션 상태의 이름이다.
    private Monster _target; // 현재 공격할 몬스터다.


    /// <summary>
    /// 활성화될 때 다음 공격을 위한 시간을 초기화한다.
    /// </summary>
    private void OnEnable()
    {
        _attackTimer = 0f;
    }

    /// <summary>
    /// 스테이지 변경 시 현재 몬스터를 공격 대상으로 설정한다.
    /// </summary>
    public void SetTarget(int stage)
    {
        _target = StageManager.Instance.CurrentMonster;
    }

    /// <summary>
    /// 일정 간격으로 현재 살아 있는 몬스터를 공격한다.
    /// </summary>
    private void Update()
    {
        if (StageManager.Instance == null)
        {
            return;
        }

        if (_target == null || _target.currentMonsterData == null || _target.IsDie)
        {
            return;
        }

        _attackTimer += Time.deltaTime;
        if (_attackTimer < Mathf.Max(0.1f, SpiritManager.Instance.CurrentSpiritAttackInterval))
        {
            return;
        }

        _attackTimer = 0f;
        if (_animator != null)
        {
            _animator.Play(_attackHash, 0, 0f);
        }

        SpiritProjectile projectile = PoolManager.Instance.Get<SpiritProjectile>();
        if (projectile != null)
        {
            projectile.Launch(_target, SpiritManager.Instance.CurrentSpiritAttackPower);
        }
    }

}
