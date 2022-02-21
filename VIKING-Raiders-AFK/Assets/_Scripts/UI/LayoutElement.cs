using System;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class LayoutElement : MonoBehaviour
{
    private BaseUnitModel UnitModel { get; set; }

    [SerializeField] private Text _textContent;
    private Button selectButton;
    private Image image;
    private Color inactiveColor;
    [SerializeField] private int _unitObjectID; //<

    [SerializeField] private bool isSelected;

    private void Awake()
    {
        selectButton = GetComponent<Button>();
        image = GetComponent<Image>();
        inactiveColor = image.color;
    }

    private void OnEnable()
    {
        selectButton.onClick.AddListener(SelectElement);
    }

    private void OnDisable()
    {
        selectButton.onClick.RemoveListener(SelectElement);
    }

    public void Init(BaseUnitModel model)
    {
        UnitModel = model;
        _textContent.text = UnitModel.characterName;
    }

    private void SelectElement()
    {
        if (isSelected)
        {
            image.color = inactiveColor;
            SpawnContoller.Instance.RemoveUnit(UnitModel, _unitObjectID);
        }
        else
        {
            image.color = Color.red;
            _unitObjectID = SpawnContoller.Instance.SpawnUnit(UnitModel);
        }

        Debug.Log("Switch!");
        isSelected = !isSelected;
    }
}