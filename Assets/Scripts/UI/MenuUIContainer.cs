using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuUIContainer : MonoBehaviour
{
    [SerializeField] private List<Button> _menuButtonList = new List<Button>();
    public List<Button> MenuButtonList => _menuButtonList;

    [SerializeField] private GridLayoutGroup _gridLayoutGroup;

    private float _width;
    private float _height;

    void Awake()
    {
        for(int i=0; i<transform.childCount; i++)
        {
            Button menuButton = transform.GetChild(0).GetComponent<Button>();
            _menuButtonList.Add(menuButton);
        }
    }

    void Start()
    {
        _width = GetComponent<RectTransform>().rect.width;
        _height = GetComponent<RectTransform>().rect.height;

        _gridLayoutGroup.cellSize = new Vector2(_width / transform.childCount, _height);
    }

    public void OnClickedMenuBtn(int index)
    {
        HUDManager.Instance.ShowContentPanelUI(index);
    }
}
