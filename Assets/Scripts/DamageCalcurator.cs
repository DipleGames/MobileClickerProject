using UnityEngine;
using System;

// 바로 연산 로직을 사용하기위해 static 키워드 사용
public static class DamageCalcurator
{
    public static double CalculateDamage(double attackPower, double skillMultiplier, double defense)
    {
        double damage = attackPower * skillMultiplier / defense; // 임시 계산 로직

        return Math.Max(1, damage);
    }
}
