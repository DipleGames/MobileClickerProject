using UnityEngine;
using UnityEngine.UI;

public class SkillBtn : MonoBehaviour
{
    [SerializeField] private SkillRuntime _skill;

    [SerializeField] private Button skillBtn;
    [SerializeField] private GameObject lockOverlay;

    void Awake()
    {
        skillBtn.onClick.AddListener(OnClickedSkillBtn);
    }

    public void SetSkill(SkillRuntime skill)
    {
        _skill = skill;
    }

    public void OnClickedSkillBtn()
    {
        if (_skill == null)
            return;

        _skill.Data.isUnlocked = true;
        lockOverlay.SetActive(false);
    }
}