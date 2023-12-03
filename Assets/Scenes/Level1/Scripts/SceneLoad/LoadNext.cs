using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadNext : MonoBehaviour
{
    [Header("Loading Panel")]
    [SerializeField] private GameObject _loading_bar_gameobject;
    [SerializeField] private Slider _loadingSlider;

    [Header("Scenes to Load")]
    [SerializeField] private string _levelScene = "Level(2-0)";
    [SerializeField] private string _levelSceneAdditive = "Level(2-2)";

    private List<AsyncOperation> scenesToLoad = new List<AsyncOperation>();
    public LoadScene loadScene;
    void Start()
    {
        SavePointSerial.CurrentSavePointIndex = 5;
        StartCoroutine(ProgressLoadingBar());
    }
    private IEnumerator ProgressLoadingBar()
    {
        _loading_bar_gameobject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        scenesToLoad.Add(SceneManager.LoadSceneAsync(_levelScene));
        if (_levelSceneAdditive != "")
            scenesToLoad.Add(SceneManager.LoadSceneAsync(_levelSceneAdditive, LoadSceneMode.Additive));
        float loadProgress = 0f;
        for (int i = 0; i < scenesToLoad.Count; i++)
        {
            while (!scenesToLoad[i].isDone)
            {
                loadProgress += scenesToLoad[i].progress;
                _loadingSlider.value = loadProgress / scenesToLoad.Count;
                Debug.Log(scenesToLoad[i].progress);
                yield return null;
            }
        }
    }

}
