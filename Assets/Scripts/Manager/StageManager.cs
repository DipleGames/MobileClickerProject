using UnityEngine;
using System.Collections.Generic;
using System;

public class StageManager : SingleTon<StageManager>
{
    // 나중에 이벤트 처리 용
    public event Action<int> OnChangedStage;

    [SerializeField] private StageConfig stageConfig;
    private StageDataGenerator generator;

    [Header("필드")]
    [SerializeField] private List<StageData> _stageDatas = new List<StageData>();
    [SerializeField] private int _currentStage = 1;
    [SerializeField] private bool _isClear = false;
    [SerializeField] private Monster _currentMonster;


    // 읽기전용 프로퍼티
    public List<StageData> StageDatas => _stageDatas;
    public int CurrentStage => _currentStage;
    public bool IsClear => _isClear;
    public Monster CurrentMonster => _currentMonster;

    protected override void Awake()
    {
        base.Awake();

        OnChangedStage += SpawnMonster;
        generator = new StageDataGenerator(stageConfig);
    }

    // 몬스터가 죽었다면 클리어 판정 (isDie = true = > isClear = true)
    public void OnMonsterDied(bool isDie)
    {
        _isClear = isDie;
    }

    public void ChangeStage(int stage)
    {
        _currentStage = stage;
        _isClear = false;
        OnChangedStage.Invoke(_currentStage);
    }

    public void SpawnMonster(int stage)
    {
        StageData stageData = generator.Generate(stage);
        MonsterData monsterData = stageData.monsterData;
        _currentMonster.InitMonster(monsterData);
    } 
}
