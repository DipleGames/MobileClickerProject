using UnityEngine;
using UnityEngine.UI;

public class EquipSkillBtn : MonoBehaviour
{
    [SerializeField] private SkillRuntime _skill; // 여기 들어온 스킬
    [SerializeField] private Button _equip_Btn; // 장착 버튼
    public int currentIndex;

    void Awake()
    {
        _equip_Btn.onClick.AddListener(OnClickedEquipBtn);
    }

    public void SetEquipSkillBtn(SkillRuntime skill, int index)
    {
        _skill = skill;
        currentIndex = index;
    }

    void OnClickedEquipBtn()
    {
        for(int i=0; i<6; i++)
        {
            if(i == currentIndex)
                continue; 

            if(_skill.Config == null)
                continue;

            if(SkillManagement.Instance.SkillExecutors[i].Skill == null)
                continue;

            if(SkillManagement.Instance.SkillExecutors[i].Skill.Config == null)
                continue;

            if(SkillManagement.Instance.SkillExecutors[i].Skill.Config.skillID == _skill.Config.skillID)
            {
                SkillManagement.Instance.skillManagement_ScrollView.SetActive(false);
                return; 
            }
        }
        SkillManagement.Instance.SkillExecutors[currentIndex].SetSkill(_skill);
        SkillManagement.Instance.skillManagement_ScrollView.SetActive(false);
    }
}
