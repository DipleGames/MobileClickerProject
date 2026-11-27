using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class SkillManagement : SingleTon<SkillManagement>
{
    [FormerlySerializedAs("_skillManagement_Btn")]
    [SerializeField] private Button _skillManagementBtn;
    public GameObject skillManagement_ScrollView;
    [FormerlySerializedAs("_skillManagement_Panel")]
    [SerializeField] private GameObject _skillManagementPanel;
    [FormerlySerializedAs("_equipSkill_Btn")]
    [SerializeField] private GameObject _equipSkillBtn;
    [SerializeField] private List<SkillExecutor> _skillExecutors = new();

    public List<SkillExecutor> SkillExecutors => _skillExecutors;

    // 스킬 관리 모드인가?
    private bool _isSkillManagementMode = false;
    public bool IsSkillManagementMode => _isSkillManagementMode;

    /// <summary>
    /// 싱글턴을 초기화하고 스킬 관리 버튼의 클릭을 연결한다.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        _skillManagementBtn.onClick.AddListener(OnClickedSkillManagementBtn);
    }

    /// <summary>
    /// 스킬 관리 모드를 전환하고 화면에 반영한다.
    /// </summary>
    void OnClickedSkillManagementBtn()
    {
        _isSkillManagementMode = _isSkillManagementMode ? false : true;

        OnSkillManagementMode(_isSkillManagementMode);
    }

    /// <summary>
    /// 스킬 관리 모드에 맞춰 버튼과 패널의 표시 상태를 변경한다.
    /// </summary>
    void OnSkillManagementMode(bool isSkillManagementMode)
    {
        Image image = _skillManagementBtn.GetComponent<Image>();
        Color color = image.color;
        if (isSkillManagementMode)
        {
            color.a = 1f;
            image.color = color;
        }
        else
        {
            color.a = 0.2f;
            image.color = color;
            if (skillManagement_ScrollView.activeSelf)
            {
                skillManagement_ScrollView.SetActive(false);
            }
        }

        foreach (SkillExecutor skillExecutor in SkillExecutors)
        {
            skillExecutor.transform.GetChild(1).gameObject.SetActive(isSkillManagementMode);
        }
    }

    public void OnSkillManagementPanel(int index)
    {
        SetSkillManagementPanel(index);
        skillManagement_ScrollView.SetActive(true);
    }

    /// <summary>
    /// 지정한 슬롯에 장착할 수 있는 해금 스킬 버튼을 구성한다.
    /// </summary>
    public void SetSkillManagementPanel(int index)
    {
        foreach (Transform child in _skillManagementPanel.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (var skill in SkillManager.Instance.Skiils)
        {
            if (!skill.Value.Data.isUnlocked)
            {
                continue;
            }

            // 먼저 버튼 생성
            GameObject obj = Instantiate(_equipSkillBtn, _skillManagementPanel.transform);

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

    /// <summary>
    /// 저장된 스킬 ID를 읽어 각 슬롯의 장착 상태를 복원한다.
    /// </summary>
    public void LoadEquippedSkills(int[] equippedIDs)
    {
        if (equippedIDs == null)
        {
            return;
        }

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
    