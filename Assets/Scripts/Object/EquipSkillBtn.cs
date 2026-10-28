using UnityEngine;
using UnityEngine.UI;

public class EquipSkillBtn : MonoBehaviour
{
    [SerializeField] private SkillRuntime _skill;
    [SerializeField] private Button _equip_Btn;
    public int currentIndex;

    void Awake()
    {
        _equip_Btn.onClick.AddListener(OnClickedEquipBtn);
    }

    public void SetSEquipSkillBtn(SkillRuntime skill, int index)
    {
        _skill = skill;
        currentIndex = index;
    }

    void OnClickedEquipBtn()
    {
        SkillManagement.Instance.SkillExecutors[currentIndex].SetSkill(_skill);
        SkillManagement.Instance.skillManagement_ScrollView.SetActive(false);
    }
}
