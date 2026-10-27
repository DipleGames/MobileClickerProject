using System;
using UnityEngine;

public class StageDataGenerator
{
    private StageConfig config;

    public StageDataGenerator(StageConfig config)
    {
        this.config = config;
    }

    public StageData Generate(int stage)
    {
        bool isBoss = stage % config.bossInterval == 0;

        // 몬스터 종류 결정
        MonsterData[] monsters = isBoss
            ? config.bossMonsters
            : config.normalMonsters;

        // 스테이지에 따라 몬스터 선택
        int index = (stage - 1) % monsters.Length;

        MonsterData baseMonster = monsters[index];

        // 스테이지에 따른 HP 계산
        double hp = baseMonster.monsterHP * Math.Pow(config.hpMultiplier, stage - 1);

        if (isBoss)
        {
            hp *= config.bossHpMultiplier;
        }

        // 새로운 몬스터 데이터 생성
        MonsterData monsterData = new MonsterData
        {
            monsterAOC = baseMonster.monsterAOC,
            monsterID = baseMonster.monsterID,
            monsterHP = (float)hp,
            isDie = false
        };

        return new StageData
        {
            stage = stage,
            monsterData = monsterData
        };
    }
}