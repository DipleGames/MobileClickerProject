using UnityEngine;
using UnityEngine.EventSystems;

public class ClickHandler : MonoBehaviour, IPointerDownHandler
{
    // 매개변수는 임시로 하드코딩
    public void OnPointerDown(PointerEventData eventData)
    {
        ScoreManager.Instance.AddScore(1);
        ViewManager.Instance.gameView.ShowAdditionScoreText(1);
        ViewManager.Instance.gameView.ShowTouchEffect();
        StageManager.Instance.CurrentMonster.TakeDamage(DamageCalcurator.CalculateDamage(AttackManager.Instance.CurrentAttackPower, 1f, 1f));
        Debug.Log($"{DamageCalcurator.CalculateDamage(AttackManager.Instance.CurrentAttackPower, 1f, 1f)}"); 
    }
}