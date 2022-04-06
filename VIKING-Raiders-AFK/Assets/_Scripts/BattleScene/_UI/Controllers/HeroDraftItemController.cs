using System;
using UnityEngine;
using UnityEngine.UI;

public class HeroDraftItemController : MonoBehaviour
{
    [SerializeField] private HeroDraftItemView _view;
    private User.Hero _itemHero;

    protected int buttonID => GetInstanceID();

    private bool _isSelected;

    private Button _button;
    private SpawnController _spawnController;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    public void Init(User.Hero hero)
    {
        _itemHero = hero;

        _view.Setup(_itemHero);
        _spawnController = BattleSceneController.instance.SpawnController;
    }

    protected virtual void SelectHeroItem()
    {
        if (_isSelected)
        {
            if (!BattleSceneController.instance.SpawnController.TryRemoveUnit(_itemHero, buttonID))
                return;
        }
        else
        {
            if (!_spawnController.TrySpawnUnit(_itemHero, buttonID))
                return;
        }

        _isSelected = !_isSelected;
        _view.HighlightItem(_isSelected);
    }
    
    private void OnEnable()
    {
        _button.onClick.AddListener(SelectHeroItem);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(SelectHeroItem);
    }
}