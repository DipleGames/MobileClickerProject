using UnityEngine;
using UnityEngine.EventSystems;

public class ClickHandler : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        ScoreManager.Instance.AddScore(1);
        ViewManager.Instance.gameView.ShowAdditionScoreText(1);
        ViewManager.Instance.gameView.ShowTouchEffect();
    }
}