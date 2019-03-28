using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    private static LevelController lev_con_instance;

    public int current_level = 1;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (lev_con_instance == null)
        {
            lev_con_instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
