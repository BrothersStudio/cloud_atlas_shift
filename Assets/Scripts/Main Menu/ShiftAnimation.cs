using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShiftAnimation : MonoBehaviour
{
    int current_ind = -1;
    public List<Sprite> sprites;
    SpriteRenderer sprite_renderer;

    void Awake()
    {
        sprite_renderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        StartCoroutine(Animating());
    }

    IEnumerator Animating()
    {
        while (true)
        {
            current_ind++;
            if (current_ind == sprites.Count)
            {
                current_ind = 0;
            }
            sprite_renderer.sprite = sprites[current_ind];
            yield return new WaitForSeconds(0.08f);
        }
    }
}
