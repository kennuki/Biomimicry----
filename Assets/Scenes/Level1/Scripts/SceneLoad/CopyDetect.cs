using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CopyDetect : MonoBehaviour
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
    public int SaveSceneIndex = 1;
    public int SavePointIndex = 0;
    private bool Register = false; 
    private NoDestroyList _NoDestroyList;
    private void Awake()
    {
        _NoDestroyList = GameObject.Find("NoDestroyList").GetComponent<NoDestroyList>();
    }
    private void Update()
    {
        Save();
        if(LoadScene.SceneWillChange == true)
        {
            if(Register == true)
            {
                gameObject.transform.SetParent(null);
                foreach(SaveDetail detail in Details)
                {
                    if(detail.SavePoint == SavePointSerial.CurrentSavePointIndex)
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
        if(SavePointIndex <= SavePointSerial.CurrentSavePointIndex && Register == false)
        {
            Register = true;
            StartCoroutine(DelaySaveRegister());
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
    private void Save()
    {
        if (SceneManager.GetActiveScene().buildIndex != SaveSceneIndex)
        {
            Destroy(this.gameObject);
        }
    }
    private IEnumerator DelaySaveRegister()
    {
        yield return null;
        SaveRegister();
    }
}
