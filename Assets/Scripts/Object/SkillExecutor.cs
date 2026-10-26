using UnityEngine;
using UnityEngine.UI;

public class SkillExecutor : MonoBehaviour
{
    [SerializeField] private int _skillID;
    [SerializeField] private Button skillBtn;

    void Awake()
    {
        skillBtn.onClick.AddListener(OnClickedSkillExecutor);
    }

    public void SetSkill(int skillID)
    {
        _skillID = skillID;
    }

    public void OnClickedSkillExecutor()
    {
        SkillManager.Instance.UseSkill(_skillID);
    }
}
