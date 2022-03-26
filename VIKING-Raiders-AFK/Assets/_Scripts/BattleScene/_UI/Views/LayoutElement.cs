using System;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class LayoutElement : MonoBehaviour
{
    private int ButtonID => GetInstanceID();
    private User.Hero _hero;

    [SerializeField] private Text _textContent;
    private Button selectButton;
    private Image image;

    private Color inactiveColor;

    
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

    public void Init(User.Hero hero)
    {
        _hero = hero;
        _textContent.text = _hero._heroModel.CharacterName;
    }

    private void SelectElement()
    {
        if (isSelected)
        {
            SpawnController.Instance.RemoveUnit(_hero, ButtonID);
        }
        else
        {
            if (!SpawnController.Instance.TrySpawnUnit(_hero, ButtonID))
                return;
        }

        isSelected = !isSelected;
        image.color = isSelected ? Color.red : inactiveColor;
    }
}