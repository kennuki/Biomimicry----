using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Reload : MonoBehaviour
{
    private void Start()
    {
        loadScene = GameObject.Find("LoadScene").GetComponent<LoadScene>();
        SceneManager.activeSceneChanged += OnActiveSceneChanged;
    }
    private LoadScene loadScene;
    public void ReloadScene()
    {
        Debug.Log("Click");
        LoadScene.SceneWillChange = true;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        Time.timeScale = 1;
        StartCoroutine(loadScene.ReLoadSceneDelay(1));
    }
    private void OnActiveSceneChanged(Scene previousScene, Scene newScene)
    {
        loadScene = GameObject.Find("LoadScene").GetComponent<LoadScene>();
    }
}
