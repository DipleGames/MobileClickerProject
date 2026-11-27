using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SkillExecutor : MonoBehaviour
{
    [SerializeField] private SkillRuntime _skill;
    [SerializeField] private Button _skillBtn;
    [FormerlySerializedAs("_cooldown_Slider")]
    [SerializeField] private Slider _cooldownSlider;
    [FormerlySerializedAs("empty_slot")]
    [SerializeField] private Sprite _emptySlot;

    public SkillRuntime Skill => _skill;

    /// <summary>
    /// 스킬 슬롯의 클릭을 연결하고 쿨타임 슬라이더를 초기화한다.
    /// </summary>
    void Awake()
    {
        _skillBtn.onClick.AddListener(OnClickedSkillExecutor);

        _cooldownSlider.minValue = 0f;
        _cooldownSlider.maxValue = 1f;
        _cooldownSlider.value = 0f;
        _cooldownSlider.interactable = false;
    }

    /// <summary>
    /// 장착된 스킬의 쿨타임 표시를 갱신한다.
    /// </summary>
    private void Update()
    {
        if (_skill == null || _skill.Config == null)
        {
            return;
        }

        UpdateCooldownUI();
    }

    /// <summary>
    /// 스킬을 슬롯에 장착하고 아이콘을 표시한다.
    /// </summary>
    public void SetSkill(SkillRuntime skill)
    {
        _skill = skill;
        Image image = GetComponent<Image>();

        if (_skill == null)
        {
            image.sprite = _emptySlot;
            _cooldownSlider.value = 0f;
            return;
        }

        image.sprite = _skill.Config.skillIcon;
    }

    /// <summary>
    /// 슬롯의 스킬을 해제하고 빈 슬롯 상태로 되돌린다.
    /// </summary>
    public void RemoveSkill()
    {
        if (_skill.RemainingCoolTime > 0f)
        {
            return;
        }

        _skill = null;
        GetComponent<Image>().sprite = _emptySlot;
    }

    /// <summary>
    /// 남은 쿨타임을 슬롯의 슬라이더에 표시한다.
    /// </summary>
    private void UpdateCooldownUI()
    {
        float coolTime = _skill.Config.skillCoolTime;
        float remaining = _skill.RemainingCoolTime;

        _cooldownSlider.value = coolTime > 0f ? Mathf.Clamp01(remaining / coolTime) : 0f;
    }

    /// <summary>
    /// 슬롯 클릭 시 관리 모드에 따라 스킬을 사용하거나 장착을 해제한다.
    /// </summary>
    public void OnClickedSkillExecutor()
    {
        if (!SkillManagement.Instance.IsSkillManagementMode) // 스킬 관리 모드가 아니면
        {
            SkillManager.Instance.UseSkill(_skill.Config.skillID);
        }
        else // 스킬 관리 모드면
        {
            RemoveSkill();
        }
    }
}
