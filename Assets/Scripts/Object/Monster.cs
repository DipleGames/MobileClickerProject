using UnityEngine;
using UnityEngine.UI;

public class Monster : MonoBehaviour
{
    public MonsterData currentMonsterData;

    [SerializeField] private AnimatorOverrideController _monsterAOC;
    [SerializeField] private int _monsterID;
    [SerializeField] private float _monsterHP;
    [SerializeField] private float _currentmonsterHP;
    [SerializeField] private bool _isDie;

    [SerializeField] private Animator _anim;

    public float CurrentMonsterHP
    {
        get => _currentmonsterHP;
        set
        {
            _currentmonsterHP = Mathf.Max(0, value);
            if(_currentmonsterHP <= 0)
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
                return;

            _isDie = value;

            if (_isDie)
            {
                StageManager.Instance.OnMonsterDied(true);
            }
        }
    }

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

        ViewManager.Instance.monsterView.UpdateMonsterHP(_currentmonsterHP, _monsterHP);
    }

    private static readonly int HitHash = Animator.StringToHash("Hit");
    public void TakeDamage(double damage)
    {
        if (IsDie) return;

        CurrentMonsterHP -= (float)damage;
        _anim.Play(HitHash, 0, 0f); // Hit 실행
        ViewManager.Instance.monsterView.UpdateMonsterHP(_currentmonsterHP, _monsterHP);
    }
}
