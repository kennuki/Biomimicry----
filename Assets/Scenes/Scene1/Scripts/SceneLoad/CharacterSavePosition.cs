using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSavePosition : MonoBehaviour
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
    public List<SaveDetail> Details;
    private void Update()
    {
        /*if (LoadScene.SceneWillChange == true)
        {

            gameObject.transform.SetParent(null);
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

        }*/
    }
    
}
