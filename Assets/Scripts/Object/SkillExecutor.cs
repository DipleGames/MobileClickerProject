using TMPro;
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
    [SerializeField] private Sprite _equippedFrame;

    private Image _slotImage;
    private Image _skillIcon;
    private TextMeshProUGUI _cooldownLabel;
    private int _displayedSeconds;

    public SkillRuntime Skill => _skill;

    /// <summary>
    /// 슬롯 클릭과 쿨타임 슬라이더를 초기화하고 금테 안에 아이콘을 표시할 이미지를 만든다.
    /// </summary>
    void Awake()
    {
        _skillBtn.onClick.AddListener(OnClickedSkillExecutor);

        _cooldownSlider.minValue = 0f;
        _cooldownSlider.maxValue = 1f;
        _cooldownSlider.value = 0f;
        _cooldownSlider.interactable = false;

        _slotImage = GetComponent<Image>();
        GameObject icon = new GameObject("Skill Icon", typeof(RectTransform), typeof(Image));
        icon.layer = gameObject.layer;
        RectTransform iconRect = (RectTransform)icon.transform;
        iconRect.SetParent(_cooldownSlider.transform, false);
        iconRect.SetAsFirstSibling();
        iconRect.anchorMin = new Vector2(0.17f, 0.17f);
        iconRect.anchorMax = new Vector2(0.83f, 0.83f);
        iconRect.offsetMin = Vector2.zero;
        iconRect.offsetMax = Vector2.zero;

        _skillIcon = icon.GetComponent<Image>();
        _skillIcon.preserveAspect = true;
        _skillIcon.raycastTarget = false;
        InitializeCooldownDisplay();
        SetSkill(_skill);
    }

    /// <summary>
    /// 금색 진행바를 슬롯 아래에 배치하고 중앙에 남은 초를 표시할 텍스트를 만든다.
    /// </summary>
    private void InitializeCooldownDisplay()
    {
        RectTransform fillArea = (RectTransform)_cooldownSlider.fillRect.parent;
        fillArea.anchorMin = new Vector2(0.15f, 0.13f);
        fillArea.anchorMax = new Vector2(0.85f, 0.2f);
        fillArea.offsetMin = Vector2.zero;
        fillArea.offsetMax = Vector2.zero;

        _cooldownSlider.direction = Slider.Direction.LeftToRight;
        Image fill = _cooldownSlider.fillRect.GetComponent<Image>();
        fill.sprite = null;
        fill.color = new Color32(255, 193, 76, 255);
        fill.raycastTarget = false;

        GameObject label = new GameObject("Cooldown Seconds", typeof(RectTransform), typeof(TextMeshProUGUI));
        label.layer = gameObject.layer;
        RectTransform rect = (RectTransform)label.transform;
        rect.SetParent(_cooldownSlider.transform, false);
        rect.anchorMin = new Vector2(0.14f, 0.22f);
        rect.anchorMax = new Vector2(0.86f, 0.86f);
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        _cooldownLabel = label.GetComponent<TextMeshProUGUI>();
        _cooldownLabel.alignment = TextAlignmentOptions.Center;
        _cooldownLabel.fontStyle = FontStyles.Bold;
        _cooldownLabel.enableAutoSizing = true;
        _cooldownLabel.fontSizeMin = 12f;
        _cooldownLabel.fontSizeMax = 58f;
        _cooldownLabel.color = new Color32(255, 239, 199, 255);
        _cooldownLabel.raycastTarget = false;
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
        bool equipped = _skill != null && _skill.Config != null;
        _slotImage.sprite = equipped ? _equippedFrame : _emptySlot;
        _skillIcon.sprite = equipped ? _skill.Config.skillIcon : null;
        _skillIcon.enabled = _skillIcon.sprite != null;
        _skillIcon.color = Color.white;
        _cooldownSlider.value = 0f;
        _cooldownLabel.enabled = false;
        _displayedSeconds = 0;
        if (equipped)
        {
            UpdateCooldownUI();
        }
    }

    /// <summary>
    /// 슬롯의 스킬을 해제하고 빈 슬롯 상태로 되돌린다.
    /// </summary>
    public void RemoveSkill()
    {
        if (_skill == null || _skill.RemainingCoolTime > 0f)
        {
            return;
        }

        SetSkill(null);
    }

    /// <summary>
    /// 남은 쿨타임을 금색 진행바와 초 단위 숫자로 표시한다.
    /// </summary>
    private void UpdateCooldownUI()
    {
        float coolTime = _skill.Config.skillCoolTime;
        float remaining = _skill.RemainingCoolTime;

        _cooldownSlider.value = coolTime > 0f ? Mathf.Clamp01(remaining / coolTime) : 0f;
        int seconds = Mathf.CeilToInt(remaining);
        if (seconds == _displayedSeconds)
        {
            return;
        }

        _displayedSeconds = seconds;
        _skillIcon.color = seconds > 0 ? new Color32(105, 105, 105, 255) : Color.white;
        _cooldownLabel.enabled = seconds > 0;
        if (seconds > 0)
        {
            _cooldownLabel.SetText("{0:0}", seconds);
        }
    }

    /// <summary>
    /// 슬롯 클릭 시 관리 모드에 따라 스킬을 사용하거나 장착을 해제한다.
    /// </summary>
    public void OnClickedSkillExecutor()
    {
        if (_skill == null || _skill.Config == null)
        {
            return;
        }

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
