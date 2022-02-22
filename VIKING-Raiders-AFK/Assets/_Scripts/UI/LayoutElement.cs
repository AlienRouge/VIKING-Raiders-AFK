using System;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class LayoutElement : MonoBehaviour
{
    private BaseUnitModel unitModel;

    [SerializeField] private Text _textContent;
    private Button selectButton;
    private Image image;

    private Color inactiveColor;

    private int _unitObjectID;
    private bool isSelected;

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
        unitModel = model;
        _textContent.text = unitModel.characterName;
    }

    private void SelectElement()
    {
        if (isSelected)
        {
            SpawnContoller.Instance.RemoveUnit(unitModel, _unitObjectID);
        }
        else
        {
            _unitObjectID = SpawnContoller.Instance.SpawnUnit(unitModel);
            if (_unitObjectID == 0)
                return;
        }

        isSelected = !isSelected;
        image.color = isSelected ? Color.red : inactiveColor;
    }
}