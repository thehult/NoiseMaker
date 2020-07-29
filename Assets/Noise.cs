using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Noise : MonoBehaviour
{
    NoiseController parent;

    public RawImage noiseImage;

    Texture2D noiseTexture;

    int width;
    int height;

    public float Frequency { get;set; }
    public float Exponent { get; set; }
    public float Terraces { get; set; }
    public float Weight { get; set; }


    float[,] map;
    public float[,] Map
    {
        get { return map; }
    }

    public void Init(NoiseController parent)
    {
        this.parent = parent;
        Frequency = 1f;
        Exponent = 1f;
        Terraces = 0f;
        Weight = 1f;
        width = parent.textureSize.x;
        height = parent.textureSize.y;
        noiseTexture = new Texture2D(width, height);
        noiseImage.texture = noiseTexture;
        map = new float[width, height];
        GenerateTexture();
    }

    float Perlin(float x, float y)
    {
        float sample = Mathf.PerlinNoise(x * Frequency + parent.Seed, y * Frequency + parent.Seed);
        if (Exponent > 0f)
            sample = Mathf.Pow(sample, Exponent);
        if (Exponent < 0f)
            sample = 1 - Mathf.Pow(sample, -Exponent);
        if (Terraces > 0f)
            sample = Mathf.Round(Terraces * sample) / Terraces;

        return Mathf.Clamp01(sample);
    }
    float Perlin(int x, int y)
    {
        float xn = (float)x / (float)width;
        float yn = (float)y / (float)height;
        return Perlin(xn, yn);
    }

    public void GenerateTexture()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float sample = Perlin(x, y);
                map[x, y] = sample;
                noiseTexture.SetPixel(x, y, new Color(sample, sample, sample));
            }
        }
        noiseTexture.Apply();
        parent.GenerateTexture();
    }
} 
