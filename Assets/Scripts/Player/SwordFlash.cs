using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwordFlash : MonoBehaviour
{
    public void Flash()
    {
        GetComponent<Image>().enabled = true;
        Invoke("EndFlash", 0.05f);
    }

    void EndFlash()
    {
        GetComponent<Image>().enabled = false;
    }
}
