using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowersSpawner : MonoBehaviour
{
    [SerializeField] private Flower[] flowers;
    [SerializeField] private SpriteRenderer background;
    
    void Start()
    {
        Spawn();
    }

    void Spawn()
    {
        Vector2 size = background.bounds.size;

        float halfX = size.x * 0.5f;
        float x = -halfX;

        float halfY = (size.y - 0.5f) * 0.5f;

        while (x < halfX)
        {
            x += Random.Range(0.25f, 0.4f);

            Flower flower = Instantiate(flowers[Random.Range(0, flowers.Length)], transform);
            flower.transform.localPosition = new Vector2(x, Random.Range(-halfY, halfY));
        }
    }
}
