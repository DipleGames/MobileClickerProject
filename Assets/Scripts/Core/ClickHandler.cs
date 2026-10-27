using UnityEngine;
using UnityEngine.EventSystems;

public class ClickHandler : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        double damage = DamageCalcurator.CalculateDamage(AttackManager.Instance.CurrentAttackPower, 1f, 1f);

        StageManager.Instance.CurrentMonster.TakeDamage(damage);
        ScoreManager.Instance.AddScore(damage);
        ViewManager.Instance.gameView.ShowAdditionScoreText(damage);
        ViewManager.Instance.gameView.ShowTouchEffect();
    }
}