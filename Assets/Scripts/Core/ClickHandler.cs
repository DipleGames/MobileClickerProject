using UnityEngine;
using UnityEngine.EventSystems;

public class ClickHandler : MonoBehaviour, IPointerDownHandler
{
    // 매개변수는 임시로 하드코딩
    public void OnPointerDown(PointerEventData eventData)
    {
        double damage = DamageCalcurator.CalculateDamage(AttackManager.Instance.CurrentAttackPower, 1f, 1f);
        StageManager.Instance.CurrentMonster.TakeDamage(damage);
        ScoreManager.Instance.AddScore(damage);
        ViewManager.Instance.gameView.ShowAdditionScoreText(damage);
        ViewManager.Instance.gameView.ShowTouchEffect();
        Debug.Log($"{DamageCalcurator.CalculateDamage(AttackManager.Instance.CurrentAttackPower, 1f, 1f)}"); 
    }
}