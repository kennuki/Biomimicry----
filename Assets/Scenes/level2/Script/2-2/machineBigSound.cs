using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class machineBigSound : MonoBehaviour
{
    public AudioSource source;
    float counter = 0;
    float time = 5;
    private void Update()
    {
        counter += Time.deltaTime;
        if (counter > time)
        {
            source.Play();
            Random.Range(8, 20);
            counter = 0;
        }
    }
}
