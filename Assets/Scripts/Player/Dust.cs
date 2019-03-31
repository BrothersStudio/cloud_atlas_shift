using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dust : MonoBehaviour
{
    public List<Sprite> sprites;
    SpriteRenderer sprite_renderer;

    void Awake()
    {
        sprite_renderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        StartCoroutine(AnimateDust());
    }

    IEnumerator AnimateDust()
    {
        sprite_renderer.sprite = sprites[0];
        yield return new WaitForSeconds(0.1f);
        sprite_renderer.sprite = sprites[1];
        yield return new WaitForSeconds(0.1f);
        sprite_renderer.sprite = sprites[2];
        yield return new WaitForSeconds(0.1f);
        sprite_renderer.sprite = sprites[3];
        yield return new WaitForSeconds(0.1f);
        sprite_renderer.sprite = sprites[4];
        yield return new WaitForSeconds(0.1f);

        Destroy(gameObject);
    }
}
