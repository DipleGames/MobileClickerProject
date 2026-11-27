using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Monster : MonoBehaviour
{
    public MonsterData currentMonsterData;

    [SerializeField] private AnimatorOverrideController _monsterAOC;
    [SerializeField] private int _monsterID;
    [SerializeField] private float _monsterHP;
    [FormerlySerializedAs("_currentmonsterHP")]
    [SerializeField] private float _currentMonsterHP;
    [SerializeField] private bool _isDie;

    [SerializeField] private Animator _anim;

    public float CurrentMonsterHP
    {
        get => _currentMonsterHP;
        set
        {
            _currentMonsterHP = Mathf.Max(0, value);
            if (_currentMonsterHP <= 0)
            {
                IsDie = true;
            }
        }
    }

    public bool IsDie
    {
        get => _isDie;
        set
        {
            if (_isDie == value)
            {
                return;
            }

            _isDie = value;

            if (_isDie)
            {
                StageManager.Instance.OnMonsterDied(true);
            }
        }
    }

    /// <summary>
    /// 몬스터 데이터로 체력과 애니메이션을 초기화하고 HUD를 갱신한다.
    /// </summary>
    public void InitMonster(MonsterData monsterData)
    {
        currentMonsterData = monsterData;

        _monsterAOC = currentMonsterData.monsterAOC;
        _monsterID = currentMonsterData.monsterID;
        _monsterHP = currentMonsterData.monsterHP;
        _isDie = currentMonsterData.isDie;

        // Animator Controller 교체
        _anim.runtimeAnimatorController = _monsterAOC;

        // 이전 몬스터의 Hit 상태 초기화
        _anim.ResetTrigger("IsHit");

        // Idle 상태를 처음부터 재생
        _anim.Play("Idle", 0, 0f);
        _anim.Update(0f);

        CurrentMonsterHP = _monsterHP;

        ViewManager.Instance.monsterView.UpdateMonsterHP(_currentMonsterHP, _monsterHP);
    }

    private static readonly int _hitHash = Animator.StringToHash("Hit");

    /// <summary>
    /// 피해를 적용하고 피격 애니메이션, 체력 UI, 데미지 텍스트를 갱신한다.
    /// </summary>
    public void TakeDamage(bool isSkill, double damage)
    {
        if (IsDie)
        {
            return;
        }

        CurrentMonsterHP -= (float)damage;
        _anim.Play(_hitHash, 0, 0f); // Hit 실행
        ViewManager.Instance.monsterView.UpdateMonsterHP(_currentMonsterHP, _monsterHP);
        ViewManager.Instance.gameView.ShowDamageText(isSkill, damage);
    }
}
