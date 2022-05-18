using UnityEngine.Events;

namespace _Scripts.BattleScene._SceneControllers
{
    public class ShopEventController
    {
        public static UnityAction<Hero> HeroStatsFilling;
        public static UnityAction<BaseUnitModel> HeroShopStatsFilling;
        public static UnityAction<BaseUnitModel> ShopBuyAction;
    }
}