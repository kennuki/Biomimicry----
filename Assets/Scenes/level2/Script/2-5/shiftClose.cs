using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shiftClose : MonoBehaviour
{
    public GameObject obj_to_close;
    public bool closeSelf = true;   
    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift)&& Input.GetKey(KeyCode.W))
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
