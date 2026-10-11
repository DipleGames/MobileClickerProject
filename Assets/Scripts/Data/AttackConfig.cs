using UnityEngine;

[CreateAssetMenu(fileName = "AttackConfig", menuName = "Config/AttackConfig")]
public class AttackConfig : ScriptableObject
{
    [Min(0)]
    public float baseAttack = 10f;

    [Min(1)]
    public float attackMultiplier = 1.15f;

    [Min(0)]
    public float baseUpgradeCost = 10f;

    [Min(1)]
    public float upgradeCostMultiplier = 1.2f;
}