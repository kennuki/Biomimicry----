using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickClose : MonoBehaviour
{
    public GameObject obj_to_close;
    public bool closeSelf = true;
    public int mouse_index = 1;
    private void Update()
    {
        if (Input.GetMouseButtonDown(mouse_index))
        {
            if (obj_to_close.activeSelf == true)
            {
                obj_to_close.SetActive(false);
                if (closeSelf)
                    this.enabled = false;
            }

        }
    }
}
