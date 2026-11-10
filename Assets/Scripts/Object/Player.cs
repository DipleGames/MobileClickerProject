using UnityEngine;

public class Player : SingleTon<Player>
{
    [SerializeField] private Animator _anim;

    private static readonly int Attack1Hash = Animator.StringToHash("Player_Attack1");
    private static readonly int Attack2Hash = Animator.StringToHash("Player_Attack2");

    private bool _useFirstAttack = true;

    public void PlayAttack()
    {
        int attackHash = _useFirstAttack ? Attack1Hash : Attack2Hash;

        _anim.Play(attackHash, 0, 0f);

        _useFirstAttack = !_useFirstAttack;
    }
}
