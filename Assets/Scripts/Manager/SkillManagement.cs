using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class SkillManagement : SingleTon<SkillManagement>
{
    [SerializeField] private Button _skillManagement_Btn;
    public GameObject skillManagement_ScrollView;
    [SerializeField] private GameObject _skillManagement_Panel;
    [SerializeField] private GameObject _equipSkill_Btn;
    [SerializeField] private List<SkillExecutor> _skillExecutors = new();

    public List<SkillExecutor> SkillExecutors => _skillExecutors;

    // 스킬 관리 모드인가?
    private bool isSkillManagementMode = false;
    public bool IsSkillManagementMode => isSkillManagementMode;

    protected override void Awake()
    {
        base.Awake();
        _skillManagement_Btn.onClick.AddListener(OnClickedSkillManagementBtn);
    }

    // 스킬 관리버튼 누르면
    void OnClickedSkillManagementBtn()
    {
        isSkillManagementMode = isSkillManagementMode ? false : true;

        OnSkillManagementMode(isSkillManagementMode);
    }

    // 스킬 관리 모드 켰을때 스킬 관리 버튼의 알파값이 조정되며 스킬 실행기의 관리 버튼이 활성화된다.
    void OnSkillManagementMode(bool isSkillManagementMode)
    {
        Image image = _skillManagement_Btn.GetComponent<Image>();
        Color color = image.color;
        if(isSkillManagementMode)
        {
            color.a = 1f;
            image.color = color;
        }
        else
        {
            color.a = 0.2f;
            image.color = color;
            if(skillManagement_ScrollView.activeSelf)
            {
                skillManagement_ScrollView.SetActive(false);
            }
        }

        foreach(SkillExecutor skillExecutor in SkillExecutors)
        {
            skillExecutor.transform.GetChild(1).gameObject.SetActive(isSkillManagementMode);
        }
    }

    public void OnSkillManagementPanel(int index)
    {
        SetSkillManagementPanel(index);
        skillManagement_ScrollView.SetActive(true);
    }

    public void SetSkillManagementPanel(int index)
    {
        foreach (Transform child in _skillManagement_Panel.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (var skill in SkillManager.Instance.Skiils)
        {
            if (!skill.Value.Data.isUnlocked)
                continue;

            // 먼저 버튼 생성
            GameObject obj = Instantiate(_equipSkill_Btn, _skillManagement_Panel.transform);

            // 생성된 버튼에 데이터 설정
            obj.transform.GetChild(0).GetComponent<Image>().sprite = skill.Value.Config.skillIcon;

            obj.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = skill.Value.Config.skillName;

            obj.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = skill.Value.Config.skillDesc;

            obj.GetComponent<EquipSkillBtn>().SetEquipSkillBtn(skill.Value, index);
        }
    }

    // 스킬 장착상태 저장을 하기위해 필요한 로직
    public int[] GetEquippedSkillIDs()
    {
        int[] equippedIDs = new int[_skillExecutors.Count];

        for (int i = 0; i < _skillExecutors.Count; i++)
        {
            SkillExecutor executor = _skillExecutors[i];

            if (executor == null || executor.Skill == null || executor.Skill.Config == null)
            {
                equippedIDs[i] = -1;
                continue;
            }

            equippedIDs[i] = executor.Skill.Config.skillID;
        }

        return equippedIDs;
    }

    public void LoadEquippedSkills(int[] equippedIDs)
    {
        if (equippedIDs == null)
            return;

        int count = Mathf.Min(equippedIDs.Length, _skillExecutors.Count);

        for (int i = 0; i < count; i++)
        {
            int skillID = equippedIDs[i];

            if (skillID == -1)
            {
                _skillExecutors[i].SetSkill(null);
                continue;
            }

            if (SkillManager.Instance.Skiils.TryGetValue(skillID, out SkillRuntime skill)
                && skill.Data.isUnlocked)
            {
                _skillExecutors[i].SetSkill(skill);
            }
        }
    }
}
    