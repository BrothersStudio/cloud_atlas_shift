using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Heart : MonoBehaviour
{
    public Sprite filled_heart;
    public Sprite empty_heart;

    public void FlipHeartSprite()
    {
        if (GetComponent<Image>().sprite == filled_heart)
        {
            GetComponent<Image>().sprite = empty_heart;
        }
        else
        {
            GetComponent<Image>().sprite = filled_heart;
        }
    }
}
