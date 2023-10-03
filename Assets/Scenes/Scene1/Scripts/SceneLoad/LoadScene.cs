using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public static bool SceneWillChange = false;
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
            //list.SaveActiveState();
            //save.ReloadScene();
            StartCoroutine(ReLoadSceneDelay(1));
        }
    }
    public IEnumerator ReLoadSceneDelay(int Scene)
    {
        yield return null;
        Debug.Log("0.0");
        SceneManager.LoadScene(Scene);
    }
}
