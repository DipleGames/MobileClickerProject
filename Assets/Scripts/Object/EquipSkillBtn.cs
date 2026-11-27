using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class EquipSkillBtn : MonoBehaviour
{
    [SerializeField] private SkillRuntime _skill; // 여기 들어온 스킬
    [FormerlySerializedAs("_equip_Btn")]
    [SerializeField] private Button _equipBtn; // 장착 버튼
    public int currentIndex;

    /// <summary>
    /// 장착 버튼의 클릭 처리를 연결한다.
    /// </summary>
    void Awake()
    {
        _equipBtn.onClick.AddListener(OnClickedEquipBtn);
    }

    public void SetEquipSkillBtn(SkillRuntime skill, int index)
    {
        _skill = skill;
        currentIndex = index;
    }

    /// <summary>
    /// 중복 장착 여부를 확인한 뒤 선택한 슬롯에 스킬을 장착한다.
    /// </summary>
    void OnClickedEquipBtn()
    {
        for (int i = 0; i < 6; i++)
        {
            if (i == currentIndex)
            {
                continue;
            }

            if (_skill.Config == null)
            {
                continue;
            }

            if (SkillManagement.Instance.SkillExecutors[i].Skill == null)
            {
                continue;
            }

            if (SkillManagement.Instance.SkillExecutors[i].Skill.Config == null)
            {
                continue;
            }

            if (SkillManagement.Instance.SkillExecutors[i].Skill.Config.skillID == _skill.Config.skillID)
            {
                SkillManagement.Instance.skillManagement_ScrollView.SetActive(false);
                return; 
            }
        }
        SkillManagement.Instance.SkillExecutors[currentIndex].SetSkill(_skill);
        SkillManagement.Instance.skillManagement_ScrollView.SetActive(false);
    }
}
