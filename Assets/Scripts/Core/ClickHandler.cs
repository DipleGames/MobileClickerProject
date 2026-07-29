using UnityEngine;

public class ClickHandler : MonoBehaviour
{
    void Update()
    {
        if (!TryGetPointerDown(out Vector2 screenPos))
            return;
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        
        RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);
        if(hit.collider.name == "TouchArea")
        {
            ScoreManager.Instance.CurrentScore++;
        }
    }

   private bool TryGetPointerDown(out Vector2 screenPos)
    {
        screenPos = Vector2.zero;

        if (Application.isMobilePlatform)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    screenPos = touch.position;
                    return true;
                }
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                screenPos = Input.mousePosition;
                return true;
            }
        }

        return false;
    }

}
