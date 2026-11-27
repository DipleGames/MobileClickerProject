using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Player : SingleTon<Player>
{
    [SerializeField] private Animator _anim;
    [SerializeField] private GameObject _slash;

    private static readonly int _attack1Hash = Animator.StringToHash("Player_Attack1");
    private static readonly int _attack2Hash = Animator.StringToHash("Player_Attack2");
    private static readonly int _playerSkillAttackHash = Animator.StringToHash("Player_Skill_Attack");
    private static readonly int _playerSkillBuffHash = Animator.StringToHash("Player_Skill_Buff");

    private bool _useFirstAttack = true;


    /// <summary>
    /// 기본 공격 모션을 재생하고 검기 연출을 실행한다.
    /// </summary>
    public void PlayAttack(RectTransform target)
    {
        int attackHash = _useFirstAttack ? _attack1Hash : _attack2Hash;

        _anim.Play(attackHash, 0, 0f);
        ShowBasicAttackSlash(target);

        _useFirstAttack = !_useFirstAttack;
    }

    /// <summary>
    /// 스킬 모션 실행
    /// </summary>
    public void PlayerSkill(SkillType skillType)
    {
        int skillHash = skillType == SkillType.Active ? _playerSkillAttackHash : _playerSkillBuffHash;

        _anim.Play(skillHash, 0, 0f);
    }

    /// <summary>
    /// 플레이어의 칼 휘두름 위치에서 몬스터 몸통 방향으로 검기를 날린다.
    /// </summary>
    private void ShowBasicAttackSlash(RectTransform target)
    {
        BasicAttackSlash slash = PoolManager.Instance.Get<BasicAttackSlash>();
        slash.InitSlash(target);
    }
}
