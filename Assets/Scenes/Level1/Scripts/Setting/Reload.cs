using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Reload : MonoBehaviour
{
    public LoadScene loadScene;
    public void ReloadScene()
    {
        Debug.Log("Click");
        LoadScene.SceneWillChange = true;
        Time.timeScale = 1;
        StartCoroutine(loadScene.ReLoadSceneDelay(SceneManager.GetActiveScene().buildIndex));
    }
}
