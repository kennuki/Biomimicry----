using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skip : MonoBehaviour
{
    public static bool SkipDrama = false;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            if (SkipDrama)
            {
                SkipDrama = false;
            }
            else
            {
                SkipDrama = true;
            }
        }
        if (SkipDrama == true)
        {
            Time.timeScale = 5;
        }
        else
        {
            Time.timeScale = 1;
        }

    }
}
