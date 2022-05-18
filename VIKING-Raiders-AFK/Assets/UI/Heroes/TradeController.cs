using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradeController
{
    private User _user;

    public TradeController(User user)
    {
        _user = user;
    }

    public bool CanBuy(BaseUnitModel tradeTarget)
    {
        bool canBuy = true; 
        foreach (var hero in _user.heroList)
        {
            if (hero._heroModel.CharacterName == tradeTarget.CharacterName)
            {
                canBuy = false;
                break;
            }   
        }
        return (_user._money - tradeTarget.CharacterPrice) >= 0 && canBuy;
    }
    
    public void Buy(BaseUnitModel tradeTarget)
    {
        _user._money -= tradeTarget.CharacterPrice;
        _user.heroList.Add(new Hero
        {
            _heroLevel = 1,
            _heroModel = tradeTarget
        });
    }
}
