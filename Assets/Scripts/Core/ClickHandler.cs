using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickHandler : MonoBehaviour, IPointerDownHandler
{
    /// <summary>
    /// 플레이어 공격과 기본 검기 연출을 재생하고 몬스터 피해와 보상을 처리한다.
    /// </summary>
    public void OnPointerDown(PointerEventData eventData)
    {
        double damage = DamageCalcurator.CalculateDamage(AttackManager.Instance.CurrentAttackPower, 1f, 1f);

        Player.Instance.PlayAttack((RectTransform)StageManager.Instance.CurrentMonster.transform);
        StageManager.Instance.CurrentMonster.TakeDamage(false, damage);
        DropManager.Instance.DropScore(damage);
        ViewManager.Instance.gameView.ShowTouchEffect();
    }
}
