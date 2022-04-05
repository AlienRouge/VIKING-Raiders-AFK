using UnityEngine;
using UnityEngine.UI;

public class LayoutElement : MonoBehaviour
{
    protected int ButtonID => GetInstanceID();
    protected User.Hero _hero;

    [SerializeField] private Text _textContent;
    protected Button selectButton;
    protected Image image;

    protected Color inactiveColor;
    protected bool isSelected;

    private void Awake()
    {
        selectButton = GetComponent<Button>();
        image = GetComponent<Image>();
        inactiveColor = image.color;
    }

   

    public void Init(User.Hero hero)
    {
        _hero = hero;
        _textContent.text = _hero._heroModel.CharacterName;
    }

    protected virtual void SelectElement()
    {
        if (isSelected)
        {
            if (!BattleSceneController.instance.SpawnController.TryRemoveUnit(_hero, ButtonID))
                return;
        }
        else
        {
            if (!BattleSceneController.instance.SpawnController.TrySpawnUnit(_hero, ButtonID))
                return;
        }

        SetIsSelected();
    }

    protected void SetIsSelected()
    {
        isSelected = !isSelected;
        image.color = isSelected ? Color.red : inactiveColor;
    }
}