using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneTrigger : MonoBehaviour
{
    [SerializeField] private string[] _scenesToLoad;
    [SerializeField] private string[] _scenesToUnload;
    GameObject player;
    private void Awake()
    {
        player = GameObject.Find("Character");
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "Character")
        {
            UnLoadScenes();
            LoadScenes();
        }
    }
    private void LoadScenes()
    {
        for(int i = 0; i < _scenesToLoad.Length; i++)
        {
            bool isSceneLoaded = false;
            for(int j = 0; j < SceneManager.sceneCount; j++)
            {
                Scene loadedScene = SceneManager.GetSceneAt(j);
                if(loadedScene.name == _scenesToLoad[i])
                {
                    isSceneLoaded = true;
                    break;
                }
            }
            if (!isSceneLoaded)
            {
                SceneManager.LoadSceneAsync(_scenesToLoad[i], LoadSceneMode.Additive);
            }
        }
    }
    private void UnLoadScenes()
    {
        for (int i = 0; i < _scenesToUnload.Length; i++)
        {
            for (int j = 0; j < SceneManager.sceneCount; j++)
            {
                Scene loadedScene = SceneManager.GetSceneAt(j);
                if (loadedScene.name == _scenesToUnload[i])
                {
                    SceneManager.UnloadSceneAsync(_scenesToUnload[i]);
                }
            }
        }
    }
    IEnumerator LoadDelay()
    {
        LoadScenes();
        yield return null;
        UnLoadScenes();
    }
}
