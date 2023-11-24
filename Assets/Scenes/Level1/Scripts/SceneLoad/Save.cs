using UnityEngine;
using UnityEngine.SceneManagement;

public class Save : MonoBehaviour
{
    public int[] SaveSceneIndex;
    private void FixedUpdate()
    {
        if (LoadScene.Instance.SceneWillChange)
        {
            foreach (int index in SaveSceneIndex)
            {
                if (index == LoadScene.Instance.SceneChangeIndex)
                {
                    if (index != SceneManager.GetActiveScene().buildIndex)
                        DontDestroyOnLoad(gameObject);
                }

            }

        }

    }

    void OnActiveSceneLoaded1(Scene scene, Scene Newscene)
    {
        Debug.Log(this.name);
        bool DontDestroy = false;
        foreach(int index in SaveSceneIndex)
        {
            if (SceneManager.GetActiveScene().buildIndex == index)
                DontDestroy = true;
        }
        if (!DontDestroy)
            Destroy(this.gameObject);
        //SceneManager.activeSceneChanged -= OnActiveSceneLoaded1;
    }
 


}
