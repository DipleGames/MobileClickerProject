using UnityEngine;
using UnityEngine.UI;

public class MonsterView : MonoBehaviour
{
    [SerializeField] private Slider _monsterHP_Slider;

    public void UpdateMonsterHP(float currentHP, float monsterHP)
    {
        _monsterHP_Slider.value = currentHP / monsterHP;
    }
}
