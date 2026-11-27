using System.Collections;
using UnityEngine;

public class BuffManager : SingleTon<BuffManager>
{
    private Coroutine _attackBuffRoutine; // 공격력 버프 종료 대기 코루틴

    /// <summary>
    /// 공격력 버프를 적용하고 재사용 시 배율과 지속시간을 갱신한다.
    /// </summary>
    public void ApplyAttackBuff(float multiplier, float duration)
    {
        if (multiplier <= 0f || duration <= 0f)
        {
            Debug.LogWarning("공격력 버프의 배율과 지속시간은 0보다 커야 합니다.");
            return;
        }

        if (_attackBuffRoutine != null)
        {
            StopCoroutine(_attackBuffRoutine);
        }

        AttackManager.Instance.SetBuffMultiplier(multiplier);
        _attackBuffRoutine = StartCoroutine(ExpireAttackBuff(duration));
    }

    /// <summary>
    /// 지속시간이 끝나면 공격력 버프 배율을 해제한다.
    /// </summary>
    private IEnumerator ExpireAttackBuff(float duration)
    {
        yield return new WaitForSeconds(duration);
        _attackBuffRoutine = null;
        AttackManager.Instance.SetBuffMultiplier(1.0);
    }

    /// <summary>
    /// 매니저가 비활성화되면 진행 중인 공격력 버프를 해제한다.
    /// </summary>
    private void OnDisable()
    {
        if (_attackBuffRoutine == null)
        {
            return;
        }

        StopCoroutine(_attackBuffRoutine);
        _attackBuffRoutine = null;
        if (AttackManager.Instance != null)
        {
            AttackManager.Instance.SetBuffMultiplier(1.0);
        }
    }
}
