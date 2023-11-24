using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadActive : MonoBehaviour
{
    public int[] ActiveSceneIndex;
    public GameObject[] Active_Object;
    private void Start()
    {
        SceneManager.activeSceneChanged += OnActiveSceneChanged;
    }

    private void OnActiveSceneChanged(Scene previousScene, Scene newScene)
    {
        bool Active = false;
        foreach (int index in ActiveSceneIndex)
        {
            if (index == newScene.buildIndex)
                Active = true;
        }
        if(Active)
            foreach (GameObject gameObject in Active_Object)
                gameObject.SetActive(true);
        if(!Active)
            foreach (GameObject gameObject in Active_Object)
                gameObject.SetActive(false);    
    }
}
