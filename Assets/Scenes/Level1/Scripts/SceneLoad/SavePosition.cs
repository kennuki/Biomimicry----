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

    private void Start()
    {
        if(Level == SceneManager.GetActiveScene().buildIndex) 
        {
            LoadPostion();
        }
        
    }
    private void LoadPostion()
    {
        foreach (SaveDetail detail in Details)
        {
            if (detail.SavePoint == SavePointSerial.CurrentSavePointIndex)
            {
                //Debug.Log(detail.Pos);
                transform.position = detail.Pos;
                if (detail.If_Rotate)
                {
                    transform.localEulerAngles = detail.Angle;
                }
            }
        }
    }
    public int Level;
    public List<SaveDetail> Details;


    
}
