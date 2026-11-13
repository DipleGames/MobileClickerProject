using UnityEngine;

[CreateAssetMenu(fileName = "DropConfig", menuName = "Config/DropConfig")]
public class DropConfig : ScriptableObject
{
    [Min(0)]
    public float baseProb = 1f;

    [Min(1)]
    public float dropProbBonus = 0.1f;

    [Min(0)]
    public float baseUpgradeCost = 10f;

    [Min(1)]
    public float upgradeCostMultiplier = 3f;
}
