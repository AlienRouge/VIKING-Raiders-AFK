using UnityEngine;

public class DemoMenuSceneController : MonoBehaviour
{
    [SerializeField] private SceneLoader _sceneLoader;
    [SerializeField] private BattleLevelCreator _levelCreator;
    [SerializeField] private User _player;
    
    public void StartDemoBattle()
    {
        var newBattleLevel = _levelCreator.GenerateBattleLevel();
        _player.currentBattleLevel = newBattleLevel;
        _sceneLoader.Load("BattleScene");
    }
}
