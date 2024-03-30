using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManagement : MonoBehaviour
{
    [Header("Main Menu Object")]
    [SerializeField] private GameObject _loading_bar_gameobject;
    [SerializeField] private Slider _loadingSlider;
    [SerializeField] private GameObject[] _object_to_hide;


    [Header("Scenes to Load")]
    [SerializeField] private string _levelScene = "Level(1-3)";
    [SerializeField] private string _levelSceneNoDestroy = "Level(1-0)";

    private List<AsyncOperation> scenesToLoad = new List<AsyncOperation>();
    private void Awake()
    {
        _loading_bar_gameobject.SetActive(false);
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(ClosePanel);
        }
        if (panel != null)
        {

            panel.SetActive(isPanelOpen);
        }
    }
    private void Update()
    {
        if (panelActive())
        {
            settingCanvas.sortingOrder = 10;
        }
        else
        {
            settingCanvas.sortingOrder = -5;
        }
        if (Input.GetKeyDown(KeyCode.Escape) && panel != null)
        {
            isPanelOpen = !isPanelOpen;
            panel.SetActive(isPanelOpen);
        }
    }
    public void StarGame()
    {
        HideMenu();

        

        StartCoroutine(ProgressLoadingBar());
    }
    private void HideMenu()
    {
        for(int i=0; i < _object_to_hide.Length; i++)
        {
            _object_to_hide[i].SetActive(false);
        }
    }
    private IEnumerator ProgressLoadingBar()
    {
        _loading_bar_gameobject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        scenesToLoad.Add(SceneManager.LoadSceneAsync(_levelSceneNoDestroy));
        scenesToLoad.Add(SceneManager.LoadSceneAsync(_levelScene,LoadSceneMode.Additive));
        float loadProgress = 0f;
        for(int i = 0; i < scenesToLoad.Count; i++)
        {
            while (!scenesToLoad[i].isDone)
            {
                loadProgress += scenesToLoad[i].progress;
                _loadingSlider.value = loadProgress/scenesToLoad.Count;
                Debug.Log(scenesToLoad[i].progress);
                yield return null;
            }
        }
    }
    public GameObject panel;
    private bool isPanelOpen = false;
    public Canvas settingCanvas;
    public Button closeButton;
    private bool panelActive()
    {
        if (panel.activeSelf)
            return true;
        return false;
    }
    public void ClosePanel()
    {
        if (panel != null)
        {
            isPanelOpen = false;
            panel.SetActive(false);
            settingCanvas.sortingOrder = -5;
        }
    }
    public void OpenPanel()
    {
        if (panel != null)
        {
            isPanelOpen = true;
            panel.SetActive(true);
            settingCanvas.sortingOrder = 10;
        }
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
