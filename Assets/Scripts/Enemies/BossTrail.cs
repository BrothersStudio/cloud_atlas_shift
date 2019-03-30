using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTrail : MonoBehaviour
{
    public List<Sprite> animation_frames;

    SpriteRenderer sprite_renderer;

    void Awake()
    {
        sprite_renderer = GetComponent<SpriteRenderer>();
    }

    void OnEnable()
    {
        StartCoroutine(CloudAnimation());
    }

    IEnumerator CloudAnimation()
    {
        sprite_renderer.sprite = animation_frames[0];
        yield return new WaitForSeconds(0.1f);
        sprite_renderer.sprite = animation_frames[1];
        yield return new WaitForSeconds(0.1f);
        sprite_renderer.sprite = animation_frames[2];
        yield return new WaitForSeconds(0.1f);
        sprite_renderer.sprite = animation_frames[3];
        yield return new WaitForSeconds(0.1f);
        sprite_renderer.sprite = animation_frames[4];
        yield return new WaitForSeconds(0.1f);

        gameObject.SetActive(false);
    }
}
