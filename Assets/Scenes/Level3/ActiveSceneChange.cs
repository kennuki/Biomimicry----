using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActiveSceneChange : MonoBehaviour
{
    public string targetScene;
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.O))
        {
            Debug.Log("ded");
            Scene scene = SceneManager.GetSceneByName(targetScene);
            SceneManager.SetActiveScene(scene);
        }
    }
}
