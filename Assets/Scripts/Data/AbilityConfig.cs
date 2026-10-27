using UnityEngine;

[CreateAssetMenu(fileName = "AbilityConfig", menuName = "Config/AbilityConfig")]
public class AbilityConfig : ScriptableObject
{
    [Min(0)]
    public float baseAbility = 10f;

    [Min(1)]
    public float abilityMultiplier = 1.15f;

    [Min(0)]
    public float baseUpgradeCost = 10f;

    [Min(1)]
    public float upgradeCostMultiplier = 1.25f;
}