using System;
using UnityEngine;
using UnityEngine.UI;

public class HeroDraftItemController : MonoBehaviour
{
    [SerializeField] protected HeroDraftItemView _view;
    protected User.Hero _itemHero;

    protected int buttonID => GetInstanceID();

    protected bool _isSelected;

    private Button _button;
    protected SpawnController _spawnController;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    public virtual void Init(User.Hero hero)
    {
        _itemHero = hero;

        _view.Setup(_itemHero);
        _spawnController = BattleSceneController.instance.SpawnController;
    }

    protected virtual void SelectHeroItem()
    {
        if (_isSelected)
        {
            if (!_spawnController.TryRemoveUnit(_itemHero, buttonID))
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