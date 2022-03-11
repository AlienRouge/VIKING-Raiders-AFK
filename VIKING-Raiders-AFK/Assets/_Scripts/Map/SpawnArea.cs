using _Scripts.Enums;
using UnityEngine;

public class SpawnArea : MonoBehaviour
{ 
    public Team Team;
    private SpriteRenderer _spriteRenderer;
    [SerializeField] private Color _highlightColor;

   private void Start()
   {
       Init();
   }

   private void Init()
   {
       _spriteRenderer = GetComponent<SpriteRenderer>();
       _spriteRenderer.color = Color.clear;
   }

   public void HighlightSpawnArea(bool status)
   {
       _spriteRenderer.color = status ? _highlightColor : Color.clear;
   }
}
