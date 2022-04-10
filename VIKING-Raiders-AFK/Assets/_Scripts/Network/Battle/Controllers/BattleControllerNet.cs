using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class BattleControllerNet : BattleController
{
    public override void StartBattle(List<BaseUnitController> units)
    {
        Init();

        foreach (var unit in units)
        {
            _unitsByTeams[unit.ActualStats.BattleTeam].Add(unit);
        }

        foreach (var unit in units)
        {
            unit.StartBattle();
        }

    }
}