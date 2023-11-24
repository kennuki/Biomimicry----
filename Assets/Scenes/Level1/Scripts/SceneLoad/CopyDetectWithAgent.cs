using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CopyDetectWithAgent : MonoBehaviour
{
    [System.Serializable]
    public class SaveDetail
    {
        public int SavePoint;
        public Vector3 Pos;
        public bool If_Rotate;
        public Vector3 Angle;
    }
    public List<SaveDetail> Details;
    public int SavePointIndex = 0;
    private bool Register = false;
    //public bool Save_Register_At_Start = false;
    private NoDestroyList _NoDestroyList;
    private void Awake()
    {
        _NoDestroyList = GameObject.Find("NoDestroyList").GetComponent<NoDestroyList>();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void Update()
    {
        if (LoadScene.Instance.SceneWillChange == true)
        {
            if (Register == true)
            {
                gameObject.transform.SetParent(null);
                foreach (SaveDetail detail in Details)
                {
                    if (detail.SavePoint == SavePointSerial.CurrentSavePointIndex)
                    {
                        transform.position = detail.Pos;
                        if (detail.If_Rotate)
                        {
                            transform.localEulerAngles = detail.Angle;
                        }
                    }
                }
                DontDestroyOnLoad(gameObject);
            }
        }
        if (SavePointIndex <= SavePointSerial.CurrentSavePointIndex && Register == false)
        {
            Register = true;
            StartCoroutine(DelaySaveRegister());
        }
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (Register == false)
        {

            SaveItem();
        }
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    public void SaveItem()
    {
        if (_NoDestroyList == null)
        {
            _NoDestroyList = GameObject.Find("NoDestroyList").GetComponent<NoDestroyList>();
        }

        foreach (string name in _NoDestroyList.NoDestroyObj)
        {

            if (name == gameObject.name)
            {
                Destroy(gameObject, Time.deltaTime);
            }
        }
    }
    public void SaveRegister()
    {
        if (_NoDestroyList == null)
        {
            _NoDestroyList = GameObject.Find("NoDestroyList").GetComponent<NoDestroyList>();
        }
        bool CanDestroy = false;
        foreach (string name in _NoDestroyList.NoDestroyObj)
        {

            if (name == gameObject.name.ToString())
            {
                Destroy(gameObject);
                CanDestroy = true;
            }
        }
        if (CanDestroy == false)
        {

            Register = true;

            _NoDestroyList.NoDestroyObj.Add(gameObject.name);
        }
    }
    private IEnumerator DelaySaveRegister()
    {
        yield return null;
        SaveRegister();
    }
}
