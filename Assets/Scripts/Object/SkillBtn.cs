using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillBtn : MonoBehaviour
{
    [SerializeField] private SkillRuntime _skill;

    [SerializeField] private Button _skillBtn;
    public GameObject lockOverlay;
    public TextMeshProUGUI unlockCondition_text;

    void Awake()
    {
        _skillBtn.onClick.AddListener(OnClickedSkillBtn);
    }

    public void SetSkill(SkillRuntime skill)
    {
        _skill = skill;
    }

    public void OnClickedSkillBtn()
    {
        if (_skill == null)
            return;

        if (!_skill.Data.isUnlocked)
        {
            if (SkillManager.Instance.TryUnlockSkill(_skill.Config.skillID))
            {
                lockOverlay.SetActive(false);
            }

            return;
        }
    }
}