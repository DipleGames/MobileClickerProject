using UnityEngine;
using UnityEngine.UI;

public class SkillExecutor : MonoBehaviour
{
    [SerializeField] private SkillRuntime _skill;
    [SerializeField] private Button skillBtn;

    void Awake()
    {
        skillBtn.onClick.AddListener(OnClickedSkillExecutor);
    }

    public void SetSkill(SkillRuntime skill)
    {
        _skill = skill;
        GetComponent<Image>().sprite = _skill.Config.skillIcon;
    }

    public void OnClickedSkillExecutor()
    {
        SkillManager.Instance.UseSkill(_skill.Config.skillID);
    }
}
