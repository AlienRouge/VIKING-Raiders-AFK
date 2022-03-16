using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class NoiseMapRendererText : MonoBehaviour
{
    [SerializeField] public SpriteRenderer spriteRenderer = null;

    // Определение раскраски карты в зависимости от высот
    [Serializable]
    public struct TerrainLevel
    {
        public string name;
        public float height;
        public Color color;
    }
    [SerializeField] public List<TerrainLevel> terrainLevel = new List<TerrainLevel>();

    // В зависимости от типа отрисовываем шум либо в чёрно-белом, либо цветном варианте
    public void RenderMap(int width, int height, float[] noiseMap, MapType type)
    {
        if (type == MapType.Noise)
        {
            ApplyColorMap(width, height, GenerateNoiseMap(noiseMap));
        }
        else if (type == MapType.Color)
        {
            ApplyColorMap(width, height, GenerateColorMap(noiseMap));
        }
    }

    // Создание текстуры и спрайта для отображения
    private void ApplyColorMap(int width, int height, Color[] colors)
    {
        Texture2D texture = new Texture2D(width, height)
        {
            wrapMode = TextureWrapMode.Clamp,
            filterMode = FilterMode.Point
        };
        
        texture.SetPixels(colors);
        texture.Apply();
        
        spriteRenderer.sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 1.0f);;
    }

    // Преобразуем массив с данными о шуме в массив чёрно-белых цветов, для передачи в текстуру
    private Color[] GenerateNoiseMap(float[] noiseMap)
    {
        Color[] colorMap = new Color[noiseMap.Length];
        for (int i = 0; i < noiseMap.Length; i++)
        {
            colorMap[i] = Color.Lerp(Color.black, Color.white, noiseMap[i]);
        }
        return colorMap;
    }

    // Преобразуем массив с данными о шуме в массив цветов, зависящих от высоты, для передачи в текстуру
    private Color[] GenerateColorMap(float[] noiseMap)
    {
        Color[] colorMap = new Color[noiseMap.Length];
        for (int i = 0; i < noiseMap.Length; i++)
        {
            // Базовый цвет с самым высоким диапазоном значений
            colorMap[i] = terrainLevel[terrainLevel.Count-1].color;
            foreach (var level in terrainLevel)
            {
                // Если шум попадает в более низкий диапазон, то используем его
                if (noiseMap[i] < level.height)
                {
                    colorMap[i] = new Color(level.color.r, level.color.g, level.color.b);
                    break;
                }
            }
        }

        return colorMap;
    }
}