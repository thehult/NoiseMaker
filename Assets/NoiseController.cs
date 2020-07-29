using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoiseController : MonoBehaviour
{
    float seed = 1000f;
    public float Seed {
        get { return seed; }
        set
        {
            this.seed = value;
            foreach (Noise noise in noises)
                noise.GenerateTexture();
            GenerateTexture();
        }
    }

    public RawImage ResultImage;
    public Noise NoisePrefab;
    public GameObject NoiseContainer;

    public Vector2Int textureSize;

    Texture2D noiseTexture;
    List<Noise> noises = new List<Noise>();

    private void Awake()
    {
        noiseTexture = new Texture2D(textureSize.x, textureSize.y);
    }
    void Start()
    {
        ResultImage.texture = noiseTexture;
        AddNoiseLayer();
    }

    public void GenerateTexture()
    {
        float highestPoint = 0f;
        float[,] map = new float[textureSize.x, textureSize.y];
        for (int x = 0; x < textureSize.x; x++)
        {
            for (int y = 0; y < textureSize.y; y++)
            {
                float s = 0f;
                foreach (Noise noise in noises)
                {
                    s += noise.Weight * noise.Map[x, y];
                }
                map[x, y] = s;
                if (s > highestPoint)
                    highestPoint = s;
            }
        }
        for (int x = 0; x < textureSize.x; x++)
        {
            for (int y = 0; y < textureSize.y; y++)
            {
                float c = map[x, y] / highestPoint;
                noiseTexture.SetPixel(x, y, new Color(c, c, c));
            }
        }
        noiseTexture.Apply();
    }

    public void AddNoiseLayer()
    {
        Noise noise = Instantiate<Noise>(NoisePrefab, NoiseContainer.transform);
        noise.Init(this);
        noises.Add(noise);
    }

}
