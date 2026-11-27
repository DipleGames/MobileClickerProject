using UnityEngine;

public abstract class SkillEffect : ScriptableObject
{
    public AnimationClip effectClip;
    [SerializeField] private Vector2 _effectSize = new Vector2(500f, 500f);
    [SerializeField] private Vector2 _effectOffset;
    [SerializeField, Min(0.1f)] private float _effectSpeed = 1f;

    public Vector2 EffectSize => _effectSize;
    public Vector2 EffectOffset => _effectOffset;
    public float EffectSpeed => _effectSpeed;

    public abstract void Execute(SkillRuntime skill);
}
