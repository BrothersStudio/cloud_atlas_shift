using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Flash : MonoBehaviour
{
    Image image;

    bool flashing = false;
    public int dist_from_center = 0;

    Vector2 center_pos = new Vector2(480 / 2, 270 / 2);

    void Awake()
    {
        image = GetComponent<Image>();
    }

    public void Engage()
    {
        flashing = true;
        dist_from_center = 0;

        gameObject.SetActive(true);
    }

    void Update()
    {
        if (flashing)
        {
            if (!image.enabled) image.enabled = true;

            Texture2D texture = new Texture2D(480, 270);
            texture.filterMode = FilterMode.Point;
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, 480, 270), Vector2.zero);
            image.sprite = sprite;

            for (int y = 0; y < texture.height; y++)
            {
                for (int x = 0; x < texture.width; x++)
                {
                    Vector2 pos = new Vector2(x, y);
                    float dist = Vector2.Distance(pos, center_pos);
                    if (dist < dist_from_center + 20)
                    {
                        texture.SetPixel(x, y, new Color(1, 1, 1, 0));
                    }
                    else if (dist < dist_from_center)
                    {
                        texture.SetPixel(x, y, new Color(1, 1, 1, 1));
                    }
                }
            }
            texture.filterMode = FilterMode.Point;
            texture.Apply();

            dist_from_center += 40;
            if (dist_from_center >= 300)
            {
                flashing = false;
                gameObject.SetActive(false);
            }
        }
    }
}
