using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Heart : MonoBehaviour
{
    [HideInInspector]
    public bool full = true;

    public Sprite filled_heart;
    public Sprite empty_heart;

    public void FlipHeartSprite()
    {
        if (GetComponent<Image>().sprite == filled_heart)
        {
            GetComponent<Image>().sprite = empty_heart;
            full = false;
        }
        else
        {
            GetComponent<Image>().sprite = filled_heart;
            full = true;
        }
    }
}
