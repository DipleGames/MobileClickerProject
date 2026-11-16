using UnityEngine;

public abstract class SkillEffect : ScriptableObject
{
    public AnimationClip effectClip;
    public abstract void Execute(SkillRuntime skill);
}