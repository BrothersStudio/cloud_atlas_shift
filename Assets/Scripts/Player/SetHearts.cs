using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetHearts : MonoBehaviour
{
    public GameObject heart_prefab;

    void Start()
    {
        int current_health = FindObjectOfType<LevelController>().current_level + 2;
        FindObjectOfType<Player>().health = current_health;
        SpawnHearts(current_health);
    }

    void SpawnHearts(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Instantiate(heart_prefab, transform);
        }
        SetSpacing();
    }

    public void SetSpacing()
    {
        switch (transform.childCount)
        {
            case 3:
                GetComponent<VerticalLayoutGroup>().spacing = -55;
                break;
            case 4:
                GetComponent<VerticalLayoutGroup>().spacing = -40;
                break;
            case 5:
                GetComponent<VerticalLayoutGroup>().spacing = -25;
                break;
            case 6:
                GetComponent<VerticalLayoutGroup>().spacing = -10;
                break;
            case 7:
                GetComponent<VerticalLayoutGroup>().spacing = -1;
                break;
            case 8:
                GetComponent<VerticalLayoutGroup>().spacing = -1;
                break;
            case 9:
                GetComponent<VerticalLayoutGroup>().spacing = -1;
                break;
        }
    }
}
