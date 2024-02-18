using UnityEngine;
using UnityEngine.SceneManagement;

public class Save : MonoBehaviour
{
    public int[] SaveSceneIndex;
    private void FixedUpdate()
    {
        if (LoadScene.SceneWillChange)
        {
            foreach (int index in SaveSceneIndex)
            {
                if (index == LoadScene.SceneChangeIndex)
                {
                    if (index != SceneManager.GetActiveScene().buildIndex)
                        DontDestroyOnLoad(gameObject);
                }

            }

        }

    }

 


}
