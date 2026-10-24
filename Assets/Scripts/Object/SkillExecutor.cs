using UnityEngine;
using UnityEngine.UI;

public class SkillExecutor : MonoBehaviour
{
    [SerializeField] private int _skillID;
    [SerializeField] private Button skillBtn;

    void Awake()
    {
        skillBtn.onClick.AddListener(OnClickedSkillBtn);
    }

    public void SetSkill(int skillID)
    {
        _skillID = skillID;
    }

    public void OnClickedSkillBtn()
    {
        SkillManager.Instance.UseSkill(_skillID);
    }
}
