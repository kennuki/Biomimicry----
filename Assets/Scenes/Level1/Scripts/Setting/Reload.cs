using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Reload : MonoBehaviour
{
    private void Start()
    {
        //Debug.Log(this.gameObject.name);
        loadScene = LoadScene.Instance;
    }
    private LoadScene loadScene;
    public void ReloadScene()
    {
        Debug.Log("Click");
        LoadScene.Instance.SceneWillChange = true;
        Time.timeScale = 1;
        StartCoroutine(loadScene.ReLoadSceneDelay(SceneManager.GetActiveScene().buildIndex));
    }
}
