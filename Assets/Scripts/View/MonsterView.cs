using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MonsterView : MonoBehaviour
{
    [FormerlySerializedAs("_monsterHP_Slider")]
    [SerializeField] private Slider _monsterHPSlider;

    /// <summary>
    /// 현재 체력 비율을 몬스터 체력 슬라이더에 표시한다.
    /// </summary>
    public void UpdateMonsterHP(float currentHP, float monsterHP)
    {
        _monsterHPSlider.value = currentHP / monsterHP;
    }
}
