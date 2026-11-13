using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickHandler : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        double damage = DamageCalcurator.CalculateDamage(AttackManager.Instance.CurrentAttackPower, 1f, 1f);

        Player.Instance.PlayAttack();
        StageManager.Instance.CurrentMonster.TakeDamage(false, damage);
        DropManager.Instance.DropScore(damage);
        ViewManager.Instance.gameView.ShowTouchEffect();
    }
}