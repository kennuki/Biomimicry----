using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickClose : MonoBehaviour
{
    public GameObject obj_to_close;
    public bool closeSelf = true;
    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            obj_to_close.SetActive(false);
            if (closeSelf)
                this.enabled = false;
        }
    }
}
