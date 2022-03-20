using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Scripts
{
   public class Dialog
   {
      public string Title = "";
      public string Message = "";
   }

   public class DialogUI : MonoBehaviour
   {
      [SerializeField] GameObject canvas;
      [SerializeField] Text titleUIText;
      [SerializeField] Text messageUIText;
      [SerializeField] Button closeUIButton;
      [SerializeField] Button beginUIBattle;

      Dialog dialog = new Dialog();
      public static DialogUI Instance;
      void Awake()
      {
         Instance = this;
         
         closeUIButton.onClick.RemoveAllListeners();
         closeUIButton.onClick.AddListener(Hide);
      }

      public DialogUI SetTitle(string title)
      {
         dialog.Title = title;
         return Instance;
      }
      
      public DialogUI SetMessage(string message)
      {
         dialog.Message = message;
         return Instance;
      }

      public void Show()
      {
         titleUIText.text = dialog.Title;
         messageUIText.text = dialog.Message;

         dialog = new Dialog();
         canvas.SetActive(true);
      }
      
      public void Hide()
      {
         canvas.SetActive(false);
      }
      
   }
}
