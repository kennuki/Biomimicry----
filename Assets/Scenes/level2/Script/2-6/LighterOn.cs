using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LighterOn : MonoBehaviour
{
    public GameObject fire;
    public AudioSource source;
    void Update()
    {
        if(transform.parent == null)
        {
            source.Play();
            fire.SetActive(false);
        }
        else
        {
            fire.SetActive(true);
        }
    }
}
