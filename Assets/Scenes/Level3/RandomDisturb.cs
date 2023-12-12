using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomDisturb : MonoBehaviour
{
    public float minForce = 5f;
    public float maxForce = 10f;
    public float forceDuration = 1f;

    void Start()
    {
        // 在Start时启动协程
        StartCoroutine(ApplyRandomForce());
    }

    IEnumerator ApplyRandomForce()
    {
        float counter = 0;
        Vector3 randomForce=Vector3.zero;
        float randomForceMagnitude=0;
        while (true)
        {
            if (counter > forceDuration) 
            {
                randomForce = Random.onUnitSphere;
                randomForceMagnitude = Random.Range(minForce, maxForce);
                counter = 0;
            }
            GetComponent<Rigidbody>().AddForce(randomForce * randomForceMagnitude*Time.deltaTime, ForceMode.Impulse);
            counter += Time.deltaTime;
            yield return null;
        }
    }
}
