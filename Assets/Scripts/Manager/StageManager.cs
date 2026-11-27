using UnityEngine;
using UnityEngine.Serialization;
using System.Collections.Generic;
using System;
using System.Collections;

public class StageManager : SingleTon<StageManager>
{
    // 나중에 이벤트 처리 용
    public event Action<int> OnChangedStage;
    [SerializeField] private List<StageData> _stageDatas = new List<StageData>();

    [FormerlySerializedAs("stageConfig")]
    [Header("StageConfig")]
    [SerializeField] private StageConfig _stageConfig;

    [Header("필드")]
    [SerializeField] private int _currentStage = 1;
    [SerializeField] private bool _isClear = false;
    [SerializeField] private Monster _currentMonster;


    // 읽기전용 프로퍼티
    public List<StageData> StageDatas => _stageDatas;
    public int CurrentStage => _currentStage;
    public bool IsClear => _isClear;
    public Monster CurrentMonster => _currentMonster;

    
    private StageDataGenerator _generator;
    /// <summary>
    /// 싱글턴과 스테이지 데이터 생성기를 초기화한다.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();

        OnChangedStage += SpawnMonster;
        OnChangedStage += HUDManager.Instance.UpdateStageUI;
        _generator = new StageDataGenerator(_stageConfig);
    }

    /// <summary>
    /// 몬스터 처치를 처리하고 다음 스테이지로 이동한다.
    /// </summary>
    public void OnMonsterDied(bool isDie)
    {
        if (!isDie || _isClear)
        {
            return;
        }

        _isClear = true;

        // 몬스터 처치 즉시 다음 스테이지 이동
        ChangeStage(_currentStage + 1);
    }

    /// <summary>
    /// 스테이지를 변경하고 몬스터와 보스 제한시간을 설정한다.
    /// </summary>
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
        {
            _bossRoutine = StartCoroutine(BossRoutine(stage));
        }
    }
    /// <summary>
    /// 스테이지 데이터를 생성해 현재 몬스터를 초기화한다.
    /// </summary>
    public void SpawnMonster(int stage)
    {
        StageData stageData = _generator.Generate(stage);
        MonsterData monsterData = stageData.monsterData;
        _currentMonster.InitMonster(monsterData);
    }

    private Coroutine _bossRoutine;
    [Header("보스 제한시간")]
    [SerializeField] private float _bossTimeLimit = 5f;

    /// <summary>
    /// 보스 제한시간을 갱신하고 만료 시 이전 일반 스테이지로 이동한다.
    /// </summary>
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
        {
            yield break;
        }

        _bossRoutine = null;

        int stage = ((bossStage / 5) - 1) * 5 + 1;
        ChangeStage(stage);
    }
}
