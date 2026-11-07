using UnityEngine;

public class Player : SingleTon<Player>
{
    [SerializeField] private Animator _anim;

    public void PlayAttack()
    {
        _anim.Play("Player_Attack", 0, 0f);
    }
}
