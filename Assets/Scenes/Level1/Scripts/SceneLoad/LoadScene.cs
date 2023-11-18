using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public static bool SceneWillChange = false;
    public static int SceneChangeIndex = 0;
    public ActiveItemSaveList list;
    private void Awake()
    {
        SceneWillChange = false;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            SceneWillChange = true;
            SavePointSerial.CurrentSavePointIndex = 5;
            //list.SaveActiveState();
            //save.ReloadScene();
            StartCoroutine(ReLoadSceneDelay(2));
            SceneChangeIndex = 2;
        }
    }
    public IEnumerator ReLoadSceneDelay(int Scene)
    {
        yield return null;
        //Debug.Log("0.0");
        SceneManager.LoadScene(Scene);
    }
}
