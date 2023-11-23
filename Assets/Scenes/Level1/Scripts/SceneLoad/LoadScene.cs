using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public static bool SceneWillChange = false;
    public static int SceneChangeIndex = 0;
    private void Awake()
    {
        SceneWillChange = false;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            SavePointSerial.CurrentSavePointIndex = 5;
            StartCoroutine(ReLoadSceneDelay(2));
        }
    }
    public IEnumerator ReLoadSceneDelay(int Scene)
    {
        SceneWillChange = true;
        SceneChangeIndex = Scene;
        yield return null;
        //Debug.Log("0.0");
        SceneManager.LoadScene(Scene);
    }
}
