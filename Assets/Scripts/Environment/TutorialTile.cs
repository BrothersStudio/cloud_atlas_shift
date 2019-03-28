using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTile : MonoBehaviour
{
    public List<Sprite> sprites;
    SpriteRenderer sprite_renderer;

    void Start()
    {
        sprite_renderer = GetComponent<SpriteRenderer>();
        StartCoroutine(CycleSprites());
    }

    IEnumerator CycleSprites()
    {
        while (true)
        {
            sprite_renderer.sprite = sprites[0];
            yield return new WaitForSeconds(0.5f);
            sprite_renderer.sprite = sprites[1];
            yield return new WaitForSeconds(0.5f);
        }
    }
}
