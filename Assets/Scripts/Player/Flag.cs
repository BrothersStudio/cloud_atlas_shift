using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour
{
    public List<Sprite> sprites;

    SpriteRenderer sprite_renderer;

    public void Activate()
    {
        sprite_renderer = GetComponent<SpriteRenderer>();

        StartCoroutine(AnimateFlag());
    }
    
    IEnumerator AnimateFlag()
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
        sprite_renderer.sprite = sprites[5];
        yield return new WaitForSeconds(0.1f);
        sprite_renderer.sprite = sprites[6];
        yield return new WaitForSeconds(0.1f);
        sprite_renderer.sprite = sprites[7];
        yield return new WaitForSeconds(0.1f);
        sprite_renderer.sprite = sprites[8];
        yield return new WaitForSeconds(0.1f);
        sprite_renderer.sprite = sprites[9];
        yield return new WaitForSeconds(0.1f);
        sprite_renderer.sprite = sprites[10];
        yield return new WaitForSeconds(0.1f);
        sprite_renderer.sprite = sprites[11];
        yield return new WaitForSeconds(0.1f);
        sprite_renderer.sprite = sprites[12];

        GetComponent<AudioSource>().Play();
    }
}
