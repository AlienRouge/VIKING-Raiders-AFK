using UnityEngine;

public enum MapType
{
    Noise,
    Color
}

public class NoiseMapText : MonoBehaviour
{
    // Исходные данные для нашего генератора шума
    [SerializeField] public int width;
    [SerializeField] public int height;
    [SerializeField] public float scale;

    [SerializeField] public int octaves;
    [SerializeField] public float persistence;
    [SerializeField] public float lacunarity;

    [SerializeField] public int seed;
    [SerializeField] public Vector2 offset;

    [SerializeField] public MapType type = MapType.Noise;
    [SerializeField] public bool autoUpdate;

    private void Start()
    {
        GenerateMap();
    }

    public void GenerateMap()
    {
        // Генерируем карту
        float[] noiseMap = NoiseMapGenerator.GenerateNoiseMap(width, height, scale, octaves, persistence, lacunarity, offset);

        // Передаём карту в рендерер
        NoiseMapRendererText mapRenderer = FindObjectOfType<NoiseMapRendererText>();
        mapRenderer.RenderMap(width, height, noiseMap, type);
    }
}