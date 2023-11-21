using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Test : MonoBehaviour
{
    private void Start()
    {
        //Debug.Log("777");
        //SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void Awake()
    {
        //SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            Debug.Log("");
            //SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.activeSceneChanged += OnActiveSceneChanged;
        }
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        
       
    }
    private void OnActiveSceneChanged(Scene previousScene, Scene newScene)
    {
        Debug.Log("Load" + this.name + "???");
    }
}
