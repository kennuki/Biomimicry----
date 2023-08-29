using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Reload : MonoBehaviour
{
    public LoadScene loadScene;
    public void ReloadScene()
    {
        LoadScene.SceneWillChange = true;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        Time.timeScale = 1;
        StartCoroutine(loadScene.ReLoadSceneDelay(1));
    }
}
