using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LoadScene : MonoBehaviour
{
    public static LoadScene Instance;
    public static int ActiveScene_index;
    public static bool ScenesIsLoading = false;
    public bool SceneWillChange = false;
    public int SceneChangeIndex = 0;
    public List<string> sceneList = new List<string>();
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("Destroy");
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            SceneWillChange = false;
        }
    }
    void Update()
    {
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
    public IEnumerator ReLoadSceneDelay(int scene)
    {
        ScenesIsLoading = true;
        List<AsyncOperation> asyncOperations = new List<AsyncOperation>();
        SceneWillChange = true;
        SceneChangeIndex = scene;
        ActiveScene_index = scene;
        yield return null;
        //Debug.Log("0.0");
        UpdateAllSceneIndex();
        asyncOperations.Add(SceneManager.LoadSceneAsync(scene));

        foreach(string other_scene in sceneList)
        {
            asyncOperations.Add(SceneManager.LoadSceneAsync(other_scene, LoadSceneMode.Additive));
        }
        yield return null;
        float loadProgress = 0f;
        for (int i = 0; i < asyncOperations.Count; i++)
        {
            while (!asyncOperations[i].isDone)
            {
                loadProgress += asyncOperations[i].progress;
                if (asyncOperations[i].progress > 0.8f)
                {
                    ScenesIsLoading = false;
                }
                yield return null;
            }
        }

    }
    public IEnumerator ReLoadSceneDelay2(int scene)
    {
        SceneWillChange = true;
        SceneChangeIndex = scene;
        ActiveScene_index = scene;
        yield return null;
        SceneManager.LoadSceneAsync(scene);
        yield return null;
        SceneWillChange = false;
    }
    private void UpdateAllSceneIndex()
    {
        sceneList.Clear();
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            string sceneName = SceneManager.GetSceneAt(i).name;
            sceneList.Add(sceneName);
            if (sceneName == SceneManager.GetActiveScene().name)
                sceneList.Remove(sceneName);
        }
    }
}
