using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Save : MonoBehaviour
{
    public int[] SaveSceneIndex;

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        DontDestroyOnLoad(gameObject);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        bool DontDestroy = false;
        foreach(int index in SaveSceneIndex)
        {
            if (SceneManager.GetActiveScene().buildIndex == index)
                DontDestroy = true;
        }
        if (!DontDestroy)
            Destroy(this.gameObject);
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }



}
