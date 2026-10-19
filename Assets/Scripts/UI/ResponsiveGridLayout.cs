using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup))]
public class ResponsiveGridLayout : MonoBehaviour
{
    [SerializeField] private int _columnCount = 1;
    [SerializeField] private int _rowCount = 3;

    private GridLayoutGroup _grid;
    private RectTransform _rect;

    private void Awake()
    {
        _grid = GetComponent<GridLayoutGroup>();
        _rect = GetComponent<RectTransform>();

        UpdateCellSize();
    }

    private void OnRectTransformDimensionsChange()
    {
        if (_grid == null || _rect == null) return;

        UpdateCellSize();
    }

    [SerializeField, Range(0.1f, 1f)]
    private float _widthRatio = 0.9f;

    private void UpdateCellSize()
    {
        float width = _rect.rect.width;
        float height = _rect.rect.height;

        float cellWidth = width / _columnCount;
        float cellHeight = height / _rowCount;

        cellWidth *= _widthRatio;

        _grid.cellSize = new Vector2(cellWidth, cellHeight);
    }
}