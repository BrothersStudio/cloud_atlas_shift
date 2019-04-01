using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdvanceCredits : MonoBehaviour
{
    public List<Sprite> all_credits;

    public void Begin()
    {
        gameObject.SetActive(true);
        StartCoroutine(Credits());
    }

    IEnumerator Credits()
    {
        GetComponent<Image>().sprite = all_credits[0];
        yield return new WaitForSeconds(3);
        GetComponent<Image>().sprite = all_credits[1];
        yield return new WaitForSeconds(3);
        GetComponent<Image>().sprite = all_credits[2];
        yield return new WaitForSeconds(3);
        GetComponent<Image>().sprite = all_credits[3];
        yield return new WaitForSeconds(3);
        GetComponent<Image>().sprite = all_credits[4];
        yield return new WaitForSeconds(3);
        GetComponent<Image>().sprite = all_credits[5];
        yield return new WaitForSeconds(6);

        Debug.Log("Quitting!");
        Application.Quit();
    }
}
