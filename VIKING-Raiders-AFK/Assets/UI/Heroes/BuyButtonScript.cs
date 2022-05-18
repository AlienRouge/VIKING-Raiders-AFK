using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.BattleScene._SceneControllers;
using UnityEngine;
using UnityEngine.UI;

public class BuyButtonScript : MonoBehaviour
{
   private BaseUnitModel _model;
   [SerializeField] private Text _heroPrice;
   public void OnEnable()
   {
      EventController.HeroShopStatsFilling += OnHeroShopStatsFilling;
   }

   private void OnHeroShopStatsFilling(BaseUnitModel model, bool isBought)
   {
      _model = model;
      if (isBought)
      {
         _heroPrice.text = "Bought";
      }
      else
      {
         _heroPrice.text = model.CharacterPrice.ToString();
      }

   }

   public void OnDisable()
   {
      EventController.HeroShopStatsFilling -= OnHeroShopStatsFilling;
   }

   public void BuyItem()
   {
      if (_model != null)
      {
         ShopEventController.ShopBuyAction?.Invoke(_model);
      }
   }
}
