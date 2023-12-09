using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LighterOn : MonoBehaviour
{
    public GameObject fire;
    void Update()
    {
        if(transform.parent == null)
        {
            fire.SetActive(false);
        }
        else
        {
            fire.SetActive(true);
        }
    }
}
