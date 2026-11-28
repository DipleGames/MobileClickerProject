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

    /// <summary>
    /// 스킬 해금을 시도하고 버튼 표시를 갱신한다.
    /// </summary>
    public void OnClickedSkillBtn()
    {
        if (_skill == null)
        {
            return;
        }

        if (!_skill.Data.isUnlocked)
        {
            if (SkillManager.Instance.TryUnlockSkill(_skill.Config.skillID))
            {
                lockOverlay.SetActive(false);
                SaveManager.Instance.SaveData();
            }

            return;
        }
    }
}
