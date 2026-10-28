using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillManagement : SingleTon<SkillManagement>
{
    [SerializeField] private Button _skillManagement_Btn;
    public GameObject skillManagement_ScrollView;
    [SerializeField] private GameObject _skillManagement_Panel;
    [SerializeField] private GameObject _equipSkill_Btn;
    [SerializeField] private List<SkillExecutor> _skillExecutors = new();

    public List<SkillExecutor> SkillExecutors => _skillExecutors;

    protected override void Awake()
    {
        base.Awake();
        _skillManagement_Btn.onClick.AddListener(OnClickedSkillManagementBtn);
    }

    void OnClickedSkillManagementBtn()
    {
        foreach(SkillExecutor skillExecutor in SkillExecutors)
        {
            bool isOn = skillExecutor.transform.GetChild(0).gameObject.activeSelf;
            skillExecutor.transform.GetChild(0).gameObject.SetActive(!isOn);
        }
    }

    public void OnSkillManagementPanel(int index)
    {
        SetSkillManagementPanel(index);
        skillManagement_ScrollView.SetActive(true);
    }

    public void SetSkillManagementPanel(int index)
    {
        foreach(var skill in SkillManager.Instance.Skiils)
        {
            if(skill.Value.Data.isUnlocked)
            {
                _equipSkill_Btn.transform.GetChild(0).GetComponent<Image>().sprite = skill.Value.Config.skillIcon;
                _equipSkill_Btn.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = skill.Value.Config.skillName;
                _equipSkill_Btn.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = skill.Value.Config.skillDesc;
                _equipSkill_Btn.GetComponent<EquipSkillBtn>().SetSEquipSkillBtn(skill.Value, index);
                Instantiate(_equipSkill_Btn, _skillManagement_Panel.transform);
            }
        }
    }
}
    