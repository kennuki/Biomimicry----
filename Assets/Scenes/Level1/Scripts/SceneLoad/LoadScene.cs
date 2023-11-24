using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public static LoadScene Instance;
    public bool SceneWillChange = false;
    public int SceneChangeIndex = 0;
    private void Awake()
    {
        if (Instance != null)
            Destroy(this.gameObject);
        Instance = this;
        SceneWillChange = false;
        DontDestroyOnLoad(this);
    }
    void Update()
    {
        Debug.Log(SceneWillChange);
        if (Input.GetKeyDown(KeyCode.L))
        {
            SavePointSerial.CurrentSavePointIndex = 0;
            StartCoroutine(ReLoadSceneDelay(1));
        }
        if (Input.GetKeyDown(KeyCode.J))
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
        SceneWillChange = false;
        SceneManager.LoadScene(Scene);
    }
}
