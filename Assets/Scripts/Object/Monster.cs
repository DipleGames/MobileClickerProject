using UnityEngine;
using UnityEngine.UI;

public class Monster : MonoBehaviour
{
    public MonsterData currentMonsterData;

    [SerializeField] private Image _monsterImg;
    [SerializeField] private int _monsterID;
    [SerializeField] private float _monsterHP;
    [SerializeField] private float _currentmonsterHP;
    [SerializeField] private bool _isDie;

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

        _monsterImg.sprite = currentMonsterData.monsterImg;
        _monsterID = currentMonsterData.monsterID;
        _monsterHP = currentMonsterData.monsterHP;
        _isDie = currentMonsterData.isDie;

        CurrentMonsterHP = _monsterHP; // 현재체력 초기화
        ViewManager.Instance.monsterView.UpdateMonsterHP(_currentmonsterHP, _monsterHP);
    }

    public void TakeDamage(double damage)
    {
        if (IsDie) return;

        CurrentMonsterHP -= (float)damage;
        ViewManager.Instance.monsterView.UpdateMonsterHP(_currentmonsterHP, _monsterHP);
    }
}
