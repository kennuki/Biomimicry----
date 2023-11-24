using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadNext : MonoBehaviour
{
    public LoadScene loadScene;
    void Start()
    {
        SavePointSerial.CurrentSavePointIndex = 5;
        StartCoroutine(loadScene.ReLoadSceneDelay(2));
    }

    
}
