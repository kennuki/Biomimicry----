using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SavePosition : MonoBehaviour
{
    [System.Serializable]
    public class SaveDetail
    {
        public int SavePoint;
        public Vector3 Pos;
        public bool If_Rotate;
        public Vector3 Angle;
    }

    CharacterController controller;

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        LoadPostion();       
    }
    private void LoadPostion()
    {
        controller = this.GetComponent<CharacterController>();
        controller.enabled = false;
        foreach (SaveDetail detail in Details)
        {
            if (detail.SavePoint == SavePointSerial.CurrentSavePointIndex)
            {
                Debug.Log(detail.Pos);
                transform.position = detail.Pos;
                if (detail.If_Rotate)
                {
                    transform.localEulerAngles = detail.Angle;
                }
            }
        }
        controller.enabled = true;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        LoadPostion();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public int Level;
    public List<SaveDetail> Details;


    
}
