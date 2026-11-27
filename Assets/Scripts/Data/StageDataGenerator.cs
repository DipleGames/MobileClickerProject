using System;
using UnityEngine;

public class StageDataGenerator
{
    private StageConfig _config;

    /// <summary>
    /// 스테이지 생성에 사용할 설정을 보관한다.
    /// </summary>
    public StageDataGenerator(StageConfig config)
    {
        this._config = config;
    }

    /// <summary>
    /// 스테이지 번호에 맞는 몬스터와 보상 데이터를 생성한다.
    /// </summary>
    public StageData Generate(int stage)
    {
        bool isbossStage = stage % _config.bossInterval == 0;

        // 몬스터 종류 결정
        MonsterData[] monsters = isbossStage
            ? _config.bossMonsters
            : _config.normalMonsters;

        // 스테이지에 따라 몬스터 선택
        int index = (stage - 1) % monsters.Length;

        MonsterData baseMonster = monsters[index];

        // 스테이지에 따른 HP 계산
        double hp = baseMonster.monsterHP * Math.Pow(_config.hpMultiplier, stage - 1);

        if (isbossStage)
        {
            hp *= _config.bossHpMultiplier;
        }

        // 새로운 몬스터 데이터 생성
        MonsterData monsterData = new MonsterData
        {
            monsterAOC = baseMonster.monsterAOC,
            monsterID = baseMonster.monsterID,
            monsterHP = (float)hp,
            isBoss = isbossStage,
            isDie = false
        };

        return new StageData
        {
            stage = stage,
            monsterData = monsterData
        };
    }
}