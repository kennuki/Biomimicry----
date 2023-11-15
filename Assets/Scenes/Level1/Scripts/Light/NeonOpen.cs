using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeonOpen : MonoBehaviour
{
    public GameObject[] gameObjects; 
    public int n; 
    public float interval = 0.5f; 

    void Start()
    {
        n = gameObjects.Length;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer ==7)
        {
            StartCoroutine(OpenColumnsCoroutine());
        }
    }

    IEnumerator OpenColumnsCoroutine()
    {
        while (true)
        {
            for (int i = 0; i < n-1; i+=0)
            {
                for(int j = 0; j < 3; j++)
                {
                    if (gameObjects[i] != null)
                    {
                        gameObjects[i].SetActive(true);
                    }
                    i++;
                }
                yield return new WaitForSeconds(interval);
            }
            yield break;
        }
    }
}
