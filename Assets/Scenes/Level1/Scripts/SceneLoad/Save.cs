using UnityEngine;
using UnityEngine.SceneManagement;

public class Save : MonoBehaviour
{
    public int[] SaveSceneIndex;

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

    }
    private void Update()
    {
        if (LoadScene.SceneWillChange)
        {
            foreach (int index in SaveSceneIndex)
            {
                if (index == LoadScene.SceneChangeIndex) ;
            }
                    //DontDestroyOnLoad(gameObject);
        }

    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        bool DontDestroy = false;
        foreach(int index in SaveSceneIndex)
        {
            if (SceneManager.GetActiveScene().buildIndex == index)
                DontDestroy = true;
        }
        if (!DontDestroy)
            Destroy(this.gameObject);
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }



}
