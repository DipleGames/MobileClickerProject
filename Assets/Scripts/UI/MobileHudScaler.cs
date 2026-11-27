using UnityEngine;
using UnityEngine.UI;
using DeviceScreen = UnityEngine.Device.Screen;

[ExecuteAlways]
public sealed class MobileHudScaler : CanvasScaler
{
    private const float _bottomPanelFraction = 0.45f;

    [SerializeField] private RectTransform _topPanel;
    [SerializeField] private RectTransform _bottomPanel;

    private DrivenRectTransformTracker _tracker;
    private Rect _lastViewport;
    private Vector2 _lastScreenSize;

    /// <summary>
    /// 화면의 안전 영역에 맞춰 HUD 배율과 상하단 패널 위치를 계산한다.
    /// </summary>
    protected override void HandleScaleWithScreenSize()
    {
        Vector2 screenSize = new Vector2(DeviceScreen.width, DeviceScreen.height);
        if (screenSize.x <= 0f || screenSize.y <= 0f)
        {
            return;
        }

        Rect viewport = DeviceScreen.safeArea;
        if (viewport.width <= 0f || viewport.height <= 0f)
        {
            viewport = new Rect(Vector2.zero, screenSize);
        }

        float maxWidth = viewport.height * referenceResolution.x / referenceResolution.y;
        if (viewport.width > maxWidth)
        {
            viewport = new Rect(viewport.center.x - maxWidth * 0.5f, viewport.y, maxWidth, viewport.height);
        }

        SetScaleFactor(viewport.width / referenceResolution.x);
        SetReferencePixelsPerUnit(referencePixelsPerUnit);

        if (viewport == _lastViewport && screenSize == _lastScreenSize)
        {
            return;
        }

        _lastViewport = viewport;
        _lastScreenSize = screenSize;

        Vector2 min = new Vector2(viewport.xMin / screenSize.x, viewport.yMin / screenSize.y);
        Vector2 max = new Vector2(viewport.xMax / screenSize.x, viewport.yMax / screenSize.y);
        float split = Mathf.Lerp(min.y, max.y, _bottomPanelFraction);

        _tracker.Clear();
        SetPanel(_topPanel, new Vector2(min.x, split), max);
        SetPanel(_bottomPanel, min, new Vector2(max.x, split));
    }

    protected override void OnDisable()
    {
        _tracker.Clear();
        _lastScreenSize = Vector2.zero;
        base.OnDisable();
    }

    /// <summary>
    /// 지정한 앵커 범위를 채우도록 패널 위치와 크기를 설정한다.
    /// </summary>
    private void SetPanel(RectTransform panel, Vector2 min, Vector2 max)
    {
        if (panel == null)
        {
            return;
        }

        var properties = DrivenTransformProperties.Anchors | DrivenTransformProperties.AnchoredPosition | DrivenTransformProperties.SizeDelta;
        _tracker.Add(this, panel, properties);
        panel.anchorMin = min;
        panel.anchorMax = max;
        panel.offsetMin = Vector2.zero;
        panel.offsetMax = Vector2.zero;
    }
}
