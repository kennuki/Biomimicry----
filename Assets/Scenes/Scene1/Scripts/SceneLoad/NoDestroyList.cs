using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoDestroyList : MonoBehaviour
{
    public static NoDestroyList instance;
    public List<string> NoDestroyObj = new List<string>();
    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }
    private void Update()
    {
        
    }
}
