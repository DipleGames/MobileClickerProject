using UnityEngine;

public class Monster : MonoBehaviour
{
    public MonsterData currentMonsterData;

    [SerializeField] private int _monsterID;
    [SerializeField] private float _monsterHP;
    [SerializeField] private bool _isDie;

    public float MonsterHP
    {
        get => _monsterHP;
        set
        {
            _monsterHP = Mathf.Max(0, value);
            if(_monsterHP <= 0)
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

        _monsterID = currentMonsterData.monsterID;
        _monsterHP = currentMonsterData.monsterHP;
        _isDie = currentMonsterData.isDie;
    }

    public void TakeDamage(float damage)
    {
        if (IsDie) return;

        MonsterHP -= damage;
        Debug.Log($"{MonsterHP}");
    }
}
