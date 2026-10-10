using UnityEngine;

[CreateAssetMenu(fileName = "StageConfig", menuName = "Stage/StageConfig")]
public class StageConfig : ScriptableObject
{
    [Header("Stage Settings")]
    public int bossInterval = 5;

    [Header("Monster Scaling")]
    public float hpMultiplier = 1.15f;
    public float bossHpMultiplier = 10f;

    [Header("Monster Data")]
    public MonsterData[] normalMonsters;
    public MonsterData[] bossMonsters;
}