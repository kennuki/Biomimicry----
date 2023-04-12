using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricDoorOpen : MonoBehaviour
{
    public GameObject door;
    void Start()
    {
        StartCoroutine(Open(3));
    }

    
    void Update()
    {
        
    }
    private IEnumerator Open(float second)
    {
        for (float i = 0; i < second; i += Time.deltaTime)
        {
            door.transform.Translate(0, 1 * Time.deltaTime, 0);
            yield return new WaitForSeconds(Time.deltaTime);
        }

    }
}
