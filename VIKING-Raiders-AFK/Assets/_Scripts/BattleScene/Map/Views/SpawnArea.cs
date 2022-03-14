using System;
using _Scripts.Enums;
using UnityEngine;

public class SpawnArea : MonoBehaviour
{ 
    public Team Team;
    private SpriteRenderer _spriteRenderer;
    [SerializeField] private Color _highlightColor;

   private void Awake()
   {
       Init();
   }

   private void Init()
   {
       _spriteRenderer = GetComponent<SpriteRenderer>();
       _spriteRenderer.color = Color.clear;
   }

   private void OnEnable()
   {
       EventController.unitDrag += HighlightSpawnArea;
   }

   private void OnDisable()
   {
       EventController.unitDrag -= HighlightSpawnArea;
   }

   private void HighlightSpawnArea(Team team)
   {
       if (Team == team)
       {
           _spriteRenderer.color = _spriteRenderer.color == Color.clear ? _highlightColor : Color.clear;
       }
   }
}
