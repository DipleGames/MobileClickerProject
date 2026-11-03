using UnityEngine;
using UnityEngine.UI;

public class SkillExecutor : MonoBehaviour
{
    [SerializeField] private SkillRuntime _skill;
    [SerializeField] private Button _skillBtn;
    [SerializeField] private Slider _cooldown_Slider;
    [SerializeField] private Sprite empty_slot;

    public SkillRuntime Skill => _skill;

    void Awake()
    {
        _skillBtn.onClick.AddListener(OnClickedSkillExecutor);

        _cooldown_Slider.minValue = 0f;
        _cooldown_Slider.maxValue = 1f;
        _cooldown_Slider.value = 0f;
        _cooldown_Slider.interactable = false;
    }

    private void Update()
    {
        if (_skill == null || _skill.Config == null)
            return;

        UpdateCooldownUI();
    }

    // 스킬 세팅(장착)하기
    public void SetSkill(SkillRuntime skill)
    {
        _skill = skill;
        Image image = GetComponent<Image>();

        if (_skill == null)
        {
            image.sprite = empty_slot;
            _cooldown_Slider.value = 0f;
            return;
        }

        image.sprite = _skill.Config.skillIcon;
    }

    // 스킬 제거하기
    public void RemoveSkill()
    {
        if(_skill.RemainingCoolTime > 0f)
        {
            return;
        }

        _skill = null;
        GetComponent<Image>().sprite = empty_slot;
    }

    // 쿨타임 slider 작동
    private void UpdateCooldownUI()
    {
        float coolTime = _skill.Config.skillCoolTime;
        float remaining = _skill.RemainingCoolTime;

        _cooldown_Slider.value = coolTime > 0f
            ? Mathf.Clamp01(remaining / coolTime)
            : 0f;
    }

    public void OnClickedSkillExecutor()
    {
        if(!SkillManagement.Instance.IsSkillManagementMode) // 스킬 관리 모드가 아니면
        {
            SkillManager.Instance.UseSkill(_skill.Config.skillID);
        }
        else // 스킬 관리 모드면
        {
            RemoveSkill();
        }
    }
}
