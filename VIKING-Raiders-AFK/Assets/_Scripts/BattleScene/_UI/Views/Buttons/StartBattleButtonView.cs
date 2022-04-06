using UnityEngine;
using UnityEngine.UI;

public class StartBattleButtonView : ClickyButton
{
    [SerializeField] BattleSceneController _battleSceneController;
    protected override void DoOnPointerDown()
    {
       // Not implemented 
    }

    protected override void DoOnPointerUp()
    {
       _battleSceneController.OnStartButtonHandler();
    }
}
