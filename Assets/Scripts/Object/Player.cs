using UnityEngine;

public class Player : SingleTon<Player>
{
    [SerializeField] private Animator _anim;

    private static readonly int _attack1Hash = Animator.StringToHash("Player_Attack1");
    private static readonly int _attack2Hash = Animator.StringToHash("Player_Attack2");

    private bool _useFirstAttack = true;

    /// <summary>
    /// 플레이어의 공격 애니메이션을 재생한다.
    /// </summary>
    public void PlayAttack()
    {
        int attackHash = _useFirstAttack ? _attack1Hash : _attack2Hash;

        _anim.Play(attackHash, 0, 0f);

        _useFirstAttack = !_useFirstAttack;
    }
}
