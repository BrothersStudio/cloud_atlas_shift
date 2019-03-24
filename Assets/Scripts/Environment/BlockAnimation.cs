using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockAnimation : MonoBehaviour
{
    public List<Sprite> blue_sprites;
    public List<Sprite> orange_sprites;
    List<Sprite> current_sprites;

    SpriteRenderer sprite_renderer;
    
    void Awake()
    {
        sprite_renderer = GetComponent<SpriteRenderer>();

        CheckColor();
    }

    void Start()
    {
        StartCoroutine(Animate());
    }

    public void CheckColor()
    {
        if (TimeChange.current.dimension == Dimension.Blue)
        {
            current_sprites = blue_sprites;
        }
        else
        {
            current_sprites = orange_sprites;
        }
    }

    public void SwitchDimensions()
    {
        if (current_sprites == orange_sprites)
        {
            current_sprites = blue_sprites;
        }
        else
        {
            current_sprites = orange_sprites;
        }
        StopAllCoroutines();
        StartCoroutine(Animate());
    }

    IEnumerator Animate()
    {
        while (true)
        {
            sprite_renderer.sprite = current_sprites[0];
            yield return new WaitForSeconds(0.2f);
            sprite_renderer.sprite = current_sprites[1];
            yield return new WaitForSeconds(0.2f);
            sprite_renderer.sprite = current_sprites[2];
            yield return new WaitForSeconds(0.2f);
            sprite_renderer.sprite = current_sprites[3];
            yield return new WaitForSeconds(0.2f);
        }
    }
}
