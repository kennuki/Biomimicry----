using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadPanel : MonoBehaviour
{
    public GameObject dead_panel;
    public static bool ActivePanel;
    private void Update()
    {
        if (ActivePanel)
        {
            dead_panel.SetActive(true);
            ActivePanel = false;
        }
    }

}
