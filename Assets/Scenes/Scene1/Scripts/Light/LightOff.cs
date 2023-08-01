using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightOff : MonoBehaviour
{
    private Light SpotLight;
    private Transform Character;
    public float OffDistance = 40;
    private void Start()
    {
        SpotLight = this.GetComponent<Light>();
        Character = GameObject.Find("Character").transform;
    }

    
    void Update()
    {
        if (Vector3.Distance(Character.transform.position, transform.position) > OffDistance)
        {
            if(SpotLight!= null)
            {
                SpotLight.enabled = false;
            }
        }
        else
        {
            if (SpotLight != null)
            {
                if(SpotLight.enabled == false)
                {
                    SpotLight.enabled = true;
                }
            }
        }
    }
}
