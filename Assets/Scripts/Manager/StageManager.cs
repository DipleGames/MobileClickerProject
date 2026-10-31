using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;

public class StageManager : SingleTon<StageManager>
{
    // 나중에 이벤트 처리 용
    public event Action<int> OnChangedStage;
    [SerializeField] private List<StageData> _stageDatas = new List<StageData>();

    [Header("StageConfig")]
    [SerializeField] private StageConfig stageConfig;

    [Header("필드")]
    [SerializeField] private int _currentStage = 1;
    [SerializeField] private bool _isClear = false;
    [SerializeField] private Monster _currentMonster;


    // 읽기전용 프로퍼티
    public List<StageData> StageDatas => _stageDatas;
    public int CurrentStage => _currentStage;
    public bool IsClear => _isClear;
    public Monster CurrentMonster => _currentMonster;

    
    private StageDataGenerator generator;
    protected override void Awake()
    {
        base.Awake();

        OnChangedStage += SpawnMonster;
        OnChangedStage += HUDManager.Instance.UpdateStageUI;
        generator = new StageDataGenerator(stageConfig);
    }

    // 몬스터가 죽었다면 클리어 판정 (isDie = true = > isClear = true)
    public void OnMonsterDied(bool isDie)
    {
        if (!isDie || _isClear)
            return;

        _isClear = true;

        // 몬스터 처치 즉시 다음 스테이지 이동
        ChangeStage(_currentStage + 1);
    }

    public void ChangeStage(int stage)
    {
        if (_bossRoutine != null)
        {
            StopCoroutine(_bossRoutine);
            _bossRoutine = null;
        }

        _currentStage = stage;
        _isClear = false;

        SpawnMonster(stage);
        OnChangedStage?.Invoke(stage);

        if (_currentMonster.currentMonsterData.isBoss)
            _bossRoutine = StartCoroutine(BossRoutine(stage));
    }
    public void SpawnMonster(int stage)
    {
        StageData stageData = generator.Generate(stage);
        MonsterData monsterData = stageData.monsterData;
        _currentMonster.InitMonster(monsterData);
    }

    private Coroutine _bossRoutine;
    [Header("보스 제한시간")]
    [SerializeField] private float _bossTimeLimit = 5f;

    IEnumerator BossRoutine(int bossStage)
    {
        float remainingTime = _bossTimeLimit;

        while (remainingTime > 0f)
        {
            HUDManager.Instance.boss_timer_text.text = $"{remainingTime:F1}s";

            remainingTime -= Time.deltaTime;
            yield return null;
        }

        HUDManager.Instance.boss_timer_text.text = "0.0s";

        if (_isClear || _currentStage != bossStage)
            yield break;

        _bossRoutine = null;

        int stage = ((bossStage / 5) - 1) * 5 + 1;
        ChangeStage(stage);
    }
}
