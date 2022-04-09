public class HeroDraftItemControllerNet : HeroDraftItemController
{
    public override void Init(User.Hero hero)
    {
        _itemHero = hero;

        _view.Setup(_itemHero);
        _spawnController = BattleSceneControllerNet.Instance.SpawnController;
    }
    
    protected override void SelectHeroItem()
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
}