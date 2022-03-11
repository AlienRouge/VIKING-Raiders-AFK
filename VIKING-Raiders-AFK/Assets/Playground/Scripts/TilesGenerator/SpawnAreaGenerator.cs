using UnityEngine;
using UnityEngine.Tilemaps;

public class SpawnAreaGenerator : MonoBehaviour
{
    [SerializeField] private Tilemap _tilemap;
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private Tile tile;

    public void BuildStartArea()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                _tilemap.SetTile(new Vector3Int(x,y,0), tile);
            }
        }
    }
}
